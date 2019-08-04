using System;

namespace datumation_products.Server.Extensions {
    public static class ConversionHelpers {
        public static long ConvertToLong (int val) {
            try {
                //int result = Convert.ToInt32(val);
                return (long) val;
            } catch (Exception e) {

                return 0;
            }
        }
        public static string ConvertForm (object val) {
            try {
                return val.ToString ();
            } catch (System.Exception e) {
                return "";

            }
        }
    }
}