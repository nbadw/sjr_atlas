using System;
using System.Collections.Generic;
using System.Text;

namespace SJRAtlas.Metadata
{
    //class NumericEncoder
    //{
    //    /*
    //     * Constants for integer encoding
    //     */
    //    static int INTEGER_SIGN_MASK = 0x80000000;

    //    /*
    //     * Constants for long encoding
    //     */
    //    static long LONG_SIGN_MASK = 0x8000000000000000L;

    //    /*
    //     * Constants for float encoding
    //     */
    //    static int FLOAT_SIGN_MASK = 0x80000000;

    //    static int FLOAT_EXPONENT_MASK = 0x7F800000;

    //    static int FLOAT_MANTISSA_MASK = 0x007FFFFF;

    //    /*
    //     * Constants for double encoding
    //     */
    //    static long DOUBLE_SIGN_MASK = 0x8000000000000000L;

    //    static long DOUBLE_EXPONENT_MASK = 0x7FF0000000000000L;

    //    static long DOUBLE_MANTISSA_MASK = 0x000FFFFFFFFFFFFFL;

    //    /**
    //     * Encode an integer into a string that orders correctly using string
    //     * comparison Integer.MIN_VALUE encodes as 00000000 and MAX_VALUE as
    //     * ffffffff.
    //     * 
    //     * @param intToEncode
    //     * @return the encoded string
    //     */
    //    public static String Encode(int intToEncode)
    //    {
    //        int replacement = intToEncode ^ INTEGER_SIGN_MASK;
    //        return encodeToHex(replacement);
    //    }

    //    /**
    //     * Encode a long into a string that orders correctly using string comparison
    //     * Long.MIN_VALUE encodes as 0000000000000000 and MAX_VALUE as
    //     * ffffffffffffffff.
    //     * 
    //     * @param longToEncode
    //     * @return - the encoded string
    //     */
    //    public static String Encode(long longToEncode)
    //    {
    //        long replacement = longToEncode ^ LONG_SIGN_MASK;
    //        return encodeToHex(replacement);
    //    }

    //    /**
    //     * Secode a long
    //     * @param hex
    //     * @return - the decoded string
    //     */
    //    public static long DecodeLong(String hex)
    //    {
    //        return decodeFromHex(hex) ^ LONG_SIGN_MASK;
    //    }
        
    //    /**
    //     * Encode a float into a string that orders correctly according to string
    //     * comparison. Note that there is no negative NaN but there are codings that
    //     * imply this. So NaN and -Infinity may not compare as expected.
    //     * 
    //     * @param floatToEncode
    //     * @return - the encoded string
    //     */
    //    public static String Encode(float floatToEncode)
    //    {
    //        int bits = Float.floatToIntBits(floatToEncode);
    //        int sign = bits & FLOAT_SIGN_MASK;
    //        int exponent = bits & FLOAT_EXPONENT_MASK;
    //        int mantissa = bits & FLOAT_MANTISSA_MASK;
    //        if (sign != 0)
    //        {
    //            exponent ^= FLOAT_EXPONENT_MASK;
    //            mantissa ^= FLOAT_MANTISSA_MASK;
    //        }
    //        sign ^= FLOAT_SIGN_MASK;
    //        int replacement = sign | exponent | mantissa;
    //        return encodeToHex(replacement);
    //    }

    //    /**
    //     * Encode a double into a string that orders correctly according to string
    //     * comparison. Note that there is no negative NaN but there are codings that
    //     * imply this. So NaN and -Infinity may not compare as expected.
    //     * 
    //     * @param doubleToEncode
    //     * @return the encoded string
    //     */
    //    public static String Encode(double doubleToEncode)
    //    {
    //        long bits = Double.doubleToLongBits(doubleToEncode);
    //        long sign = bits & DOUBLE_SIGN_MASK;
    //        long exponent = bits & DOUBLE_EXPONENT_MASK;
    //        long mantissa = bits & DOUBLE_MANTISSA_MASK;
    //        if (sign != 0)
    //        {
    //            exponent ^= DOUBLE_EXPONENT_MASK;
    //            mantissa ^= DOUBLE_MANTISSA_MASK;
    //        }
    //        sign ^= DOUBLE_SIGN_MASK;
    //        long replacement = sign | exponent | mantissa;
    //        return encodeToHex(replacement);
    //    }

    //    private static String EncodeToHex(int i)
    //    {
    //        char[] buf = new char[] { '0', '0', '0', '0', '0', '0', '0', '0' };
    //        int charPos = 8;
    //        do
    //        {
    //            buf[--charPos] = DIGITS[i & MASK];
    //            i = (i >>> 4);
    //        }
    //        while (i != 0);
    //        return new String(buf);
    //    }

    //    private static String EncodeToHex(long l)
    //    {
    //        char[] buf = new char[] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' };
    //        int charPos = 16;
    //        do
    //        {
    //            buf[--charPos] = DIGITS[(int) l & MASK];
    //            l = (l >>> 4);
    //        }
    //        while (l != 0);
    //        return new String(buf);
    //    }
        
    //    private static long DecodeFromHex(String hex)
    //    {
    //        long l = 0;
    //        long factor = 1;
    //        for(int i = 15; i >= 0; i--, factor <<= 4)
    //        {
    //            int digit = Character.digit(hex.charAt(i), 16);
    //            l += digit*factor;
    //        }
    //        return l;
    //    }

    //    private static sealed char[] DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e',
    //            'f' };

    //    private static sealed int MASK = (1 << 4) - 1;
    //}
}
