using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using datumation_products.Shared.Infrastructure.Configuration;
using datumation_products.Shared.Models;
using datumation_products.Shared.ViewModels;
using Dapper;

namespace datumation_products.Server.Data {
    public abstract class BaseDataRepo {

        public string INSERT_USER_PURCHASE = @"INSERT INTO userinvoices (itemId, userId, purchaseDate, endDate, downloadLink, fileName)
                                                    VALUES (@itemId, @userId, @purchaseDate, @endDate, @downloadLink, @fileName)";

        public string MY_ORDERS_QUERY = $@"SELECT Id,
UserId,
        ItemId,
         
         date(OrderDate),
         PaymentMethod,
       PaymentReceived,
         date(EndDate),
         City,
         StateName,
         Zip,
         AmountBilled,
         Email,
		 PhoneNumber 
FROM 
{ConfigurationFactory.Instance.Configuration().AppSettings.AppConfiguration.SqlTables.UserOrderDetailsTable} " +
            $"WHERE UserId = @userId";
        public string GET_USER_ITEMS = @"SELECT ID,
CategoryId,
Name,
Price,
ItemPictureUrl,
InternalImage,
RecordCount,
Description,
RoutePath,
StateName,
Specialty,
ItemId,
UserId,
PurchaseDate,
EndDate,
DownloadLink,
UserName,
FileName FROM datumationproductview WHERE UserName = @userName";
        public IDbConnection GetSql () {
            return new SQLiteConnection (ConfigurationFactory.Instance.Configuration ().AppSettings.AppConfiguration.ConnectionStrings.DefaultConnection);
            //return new SqlConnection(ConfigurationFactory.Instance.Configuration().DefaultConnection);
        }
        public string INSERT_USER_ORDER = @"INSERT INTO userorderdetails 
(Id, [UserId]
      ,[ItemId]
      ,[OrderDate]
      ,[PaymentMethod]
      ,[PaymentReceived]
      ,[EndDate]
      ,[City]
      ,[StateName]
      ,[Zip]
      ,[AmountBilled]
      ,[Email]
)
VALUES (@Id, @UserId, 
@ItemId,
@OrderDate, 
@PaymentMethod, 
@PaymentReceived, 
@EndDate, 
@City,
@StateName,
@Zip,
@AmountBilled,
@Email)";

    }
    public interface IDataRepo {
        Task<IEnumerable<UserItems>> GetUserItems (string userName);
        Task<IEnumerable<Items>> AllItems ();
        Task<IEnumerable<UserOrderDetails>> MyOrders (string userId);
        Task<bool> InsertPurchase (Items item, ClientCard card, ApplicationUser User);
        Task<int> GetLatestUserOrderId ();
        Task<int> GetCountByState (string state);
        Task<int> GetCountByTypeWithPar (string pt);
        Task<int> GetCountByType ();
        Task<IEnumerable<Result>> ListProviders ();
        Task<IEnumerable<string>> GetSpecialties ();
        Task<int> GetComboCount (string state, string pt);
        Task<IEnumerable<string>> GetStates ();
    }
    public class DataRepo : BaseDataRepo, IDataRepo {
        public async Task<IEnumerable<Items>> AllItems () {
            string query = $"SELECT * FROM Items";

            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    return await con.QueryAsync<Items> (query);
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [AllItems]: ERROR --==--==> {ex.Message}
                ");
                return new List<Items> ();
            }

        }
        public async Task<IEnumerable<Result>> ListProviders () {

            string query = @"SELECT * FROM Results ORDER BY NPI ASC";
            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    return await con.QueryAsync<Result> (query);
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [ListProviders]: ERROR --==--==> {ex.Message}
                ");
                return new List<Result> ();
            }

        }
        public async Task<IEnumerable<string>> GetSpecialties () {

            string query = @"SELECT DISTINCT [Specialty] FROM Items ORDER BY pt ASC";

            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    return await con.QueryAsync<string> (query);
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetSpecialties]: ERROR --==--==> {ex.Message}
                ");
                return new List<string> ();
            }
        }
        public async Task<IEnumerable<string>> GetStates () {

            string query = @"SELECT DISTINCT [StateName] FROM Results ORDER BY StateName ASC";

            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    return await con.QueryAsync<string> (query);
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetStates]: ERROR --==--==> {ex.Message}
                ");
                return new List<string> ();
            }

        }
        public async Task<int> GetCountByType () {
            int result = 0;
            string query = @"SELECT COUNT(NPI) FROM Results GROUP BY pt";

            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<int> (query);
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetCountByType]: ERROR --==--==> {ex.Message}
                ");

            }
            return result;
        }
        public async Task<int> GetCountByTypeWithPar (string pt) {
            int result = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder ();
            sb.Append ("SELECT ").Append (" COUNT(").Append ("NPI").Append (") ").AppendLine ();
            sb.Append (" FROM ").Append (" Results ").AppendLine ();
            sb.Append (" WHERE UPPER(pt) = @PT");

            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<int> (sb.ToString (), new {
                        PT = pt.ToUpper ()
                    });
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetCountByType]: ERROR --==--==> {ex.Message}
                ");

            }
            return result;
        }

        public async Task<int> GetComboCount (string state, string pt) {
            int result = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder ();
            sb.Append ("SELECT ").Append (" COUNT(").Append ("NPI").Append (") ").AppendLine ();
            sb.Append (" FROM ").Append (" Results ").AppendLine ();
            sb.Append (" WHERE UPPER(pt) = @PT AND UPPER(MailingAddressState) = @stateName ");

            try {

                using (var con = base.GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<int> (sb.ToString (), new {
                        PT = pt.ToUpper (),
                            stateName = state
                    });
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetCountByType]: ERROR --==--==> {ex.Message}
                ");

            }
            return result;
        }
        public async Task<int> GetCountByState (string stateAbbr) {
            int result = 0;
            System.Text.StringBuilder sb = new System.Text.StringBuilder ();
            sb.Append ("SELECT ").Append (" COUNT(").Append ("NPI").Append (") ").AppendLine ();
            sb.Append (" FROM ").Append (" Results ").AppendLine ();
            sb.Append (" WHERE UPPER(MailingAddressState) = @stateVal");

            try {

                using (var con = base.GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<int> (sb.ToString (), new {
                        stateVal = stateAbbr.ToUpper ()
                    });
                }
            } catch (Exception ex) {

                ConfigurationFactory.Instance.Configuration ().Logger.WriteMessage ($@"
                    DATA REPO [GetCountByType]: ERROR --==--==> {ex.Message}
                ");

            }
            return result;
        }
        public async Task<IEnumerable<UserItems>> GetUserItems (string userName) {
            string query = GET_USER_ITEMS;

            using (var con = base.GetSql ()) {
                con.Open ();
                return await con.QueryAsync<UserItems>
                    (query,
                        new { UserName = userName });
            }
        }

        public async Task<int> GetLatestUserOrderId () {
            int result = 0;
            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<int> ($@"SELECT MAX(Id) FROM 
                            {ConfigurationFactory.Instance.Configuration().AppSettings.AppConfiguration.SqlTables.UserOrderDetailsTable}");
                }
            } catch (Exception e) { }
            return result;
        }

        public async Task<bool> InsertPurchase (Items item, ClientCard card, ApplicationUser User) {
            bool result = false;
            int latestId = await GetLatestUserOrderId ();
            try {
                using (var con = GetSql ()) {
                    con.Open ();
                    result = await con.ExecuteScalarAsync<bool> (INSERT_USER_ORDER, new {
                        Id = latestId + 1,
                            UserId = User.Id,
                            OrderDate = DateTime.Now,
                            ItemId = item.Id,
                            EndDate = DateTime.Now.AddYears (1).ToShortDateString (),
                            PaymentMethod = "Credit Card",
                            PaymentReceived = true,
                            City = card.AddressCity,
                            StateName = card.AddressState,
                            Zip = card.AddressZip,
                            AmountBilled = item.Price,
                            Email = User.Email
                    });
                }
            } catch (Exception e) { }
            return result;
        }
        public async Task<IEnumerable<UserOrderDetails>> MyOrders (string userId) {
            string query = MY_ORDERS_QUERY;

            using (var con = GetSql ()) {
                con.Open ();
                try {
                    return await con.QueryAsync<UserOrderDetails> (query, new {
                        userId = userId
                    });
                } catch (Exception e) {
                    await System.IO.File.WriteAllTextAsync ("~/Data/logger.txt", e.Message);
                    return new List<UserOrderDetails> ();
                }

            }
        }
    }

}