#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright © 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
#endregion

using System;
using System.Text;
using System.Globalization;

namespace Fanap.Utilities {

	/// <summary>
	/// String handling utilities.
	/// </summary>
	public sealed class StringUtilities {

		#region Constructors
		/// <remarks>
		/// It prevents the instantiation of <see cref="StringUtilities" /> class.
		/// </remarks>
		private StringUtilities() {

		}
		#endregion

		#region Methods

        /// <summary>
        /// It counts the number of times that a given caracter appears in a string.
        /// </summary>
        /// <param name="source">
        /// The string where the character is counted.
        /// </param>
        /// <param name="find">
        /// The given character.
        /// </param>
        /// <returns>
        /// The number of times that a given caracter appears in a string.
        /// </returns>
        public static int Count( string source, char find ) {

            int ret = 0;

            foreach ( char s in source ) {
                if ( s == find ) {
                    ++ret;
                }
            }

            return ret;
        }

        /// <summary>
		/// Indicates whether or not the specified string is null or an empty string.
		/// </summary>
		/// <param name="value">
		/// The value to check.
		/// </param>
		/// <returns>
		/// true if <paramref name="value" /> is null or an empty string, otherwise false.
		/// </returns>
		public static bool IsNullOrEmpty( string value) {

			return ( ( value == null) || ( value.Length == 0) ||
				( value.Trim().Length == 0));
		}

        /// <summary>
        /// Check a string and indicates if it's a number.
        /// </summary>
        /// <param name="data">
        /// It's the string to check.
        /// </param>
        /// <returns>
        /// True if the given string is a number, otherwise false.
        /// </returns>
        public static bool IsNumber( string data ) {

            if ( IsNullOrEmpty( data ) ) {
                return false;
            }

            bool isNumber = true;

            for ( int i = 0; i < data.Length; i++ ) {
                if ( !Char.IsDigit( data, i ) ) {
                    isNumber = false;
                    break;
                }
            }

            return isNumber;
        }

		/// <summary>
		/// Converts an empty string to null.
		/// </summary>
		/// <param name="value">
		/// The value to convert.
		/// </param>
		/// <returns>
		/// null if <paramref name="value" /> is an empty string or null,
        /// otherwise it returns <paramref name="value" />.
		/// </returns>
		public static string ConvertEmptyToNull( string value) {

			if ( IsNullOrEmpty( value))
				return null;
			else
				return value;
		}

		/// <summary>
		/// Search for the character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its left.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <returns>
		/// The substring to the left of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string LeftOf( string source, char c) {

			int idx = source.IndexOf( c);

			if ( idx == -1) {
				return source;
			}

			return source.Substring( 0, idx);
		}

		/// <summary>
		/// Search for the nth character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its left.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <param name="n">
		/// The nth occurrence of <paramref name="c"/>.
		/// </param>
		/// <returns>
		/// The substring to the left of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string LeftOf( string source, char c, int n) {

			int idx = -1;

			while ( n != 0) {

				idx = source.IndexOf( c, idx + 1);

				if ( idx == -1) {
					return source;
				}

				--n;
			}
			return source.Substring( 0, idx);
		}

		/// <summary>
		/// Search for the character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its right.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <returns>
		/// The substring to the right of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string RightOf( string source, char c) {

			int idx = source.IndexOf( c);

			if ( idx == -1) {
				return string.Empty;
			}
			
			return source.Substring( idx + 1);
		}

		/// <summary>
		/// Search for the nth character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its right.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <param name="n">
		/// The nth occurrence of <paramref name="c"/>.
		/// </param>
		/// <returns>
		/// The substring to the right of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string RightOf( string source, char c, int n) {

			int idx = -1;

			while ( n != 0) {

				idx = source.IndexOf( c, idx+1);

				if ( idx == -1) {
					return string.Empty;
				}

				--n;
			}
			
			return source.Substring( idx + 1);
		}

		/// <summary>
		/// Search for the last character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its left.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <returns>
		/// The substring to the left of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string LeftOfRightmostOf( string source, char c) {

			int idx = source.LastIndexOf( c);

			if ( idx == -1) {
				return source;
			}

			return source.Substring( 0, idx);
		}

		/// <summary>
		/// Search for the last character indicated by the parameter <paramref name="c"/>
		/// in the string of characters indicated by the parameter <paramref name="source"/>,
		/// and it returns the substring that is to its right.
		/// </summary>
		/// <param name="source">
		/// It is the string to search for <paramref name="c"/>.
		/// </param>
		/// <param name="c">
		/// The character to find in <paramref name="source"/>
		/// </param>
		/// <returns>
		/// The substring to the right of <paramref name="c"/>.
		/// If <paramref name="c"/> isn't found, he function returns <paramref name="source"/>.
		/// </returns>
		public static string RightOfRightmostOf(string source, char c) {

			int idx = source.LastIndexOf( c);

			if ( idx == -1) {
				return source;
			}

			return source.Substring( idx + 1);
		}

		/// <summary>
		/// It returns the substring of characters that is between two given characters.
		/// </summary>
		/// <param name="source">
		/// It is the string of characters in which the indicated substring is searched.
		/// </param>
		/// <param name="first">
		/// It is the first character of the searched substring.
		/// </param>
		/// <param name="last">
		/// It is the last character of the searched substring.
		/// </param>
		/// <returns>
		/// The substring of characters that is between two given characters.
		/// </returns>
		/// <remarks>
		/// If <paramref name="first"/> or <paramref name="last"/> are not
		/// found, an empty string is returned.
		/// </remarks>
		public static string Between( string source, char first, char last) {

			string res = string.Empty;
			int idxStart = source.IndexOf( first);

			if ( idxStart != -1) {
				++idxStart;
				int idxEnd = source.IndexOf( last);

				if ( idxEnd != -1) {
					res = source.Substring( idxStart, idxEnd - idxStart);
				}
			}

			return res;
		}


		/// <summary>
		/// It returns the substring of characters that is between two given strings.
		/// </summary>
		/// <param name="source">
		/// It is the string of characters in which the indicated substring is searched.
		/// </param>
		/// <param name="first">
		/// It is the firts string of the searched substring.
		/// </param>
		/// <param name="last">
		/// It is the last string of the searched substring.
		/// </param>
		/// <returns>
		/// The substring of characters that is between two given strings.
		/// </returns>
		/// <remarks>
		/// If <paramref name="first"/> or <paramref name="last"/> are not
		/// found, an empty string is returned.
		/// </remarks>
		public static string Between( string source, string first, string last) {

			string res = string.Empty;
			int idxStart = source.LastIndexOf( first);

			if ( idxStart != -1) {
				idxStart += first.Length;
				int idxEnd = source.IndexOf( last, idxStart);

				if ( idxEnd != -1) {
					res = source.Substring( idxStart, idxEnd - idxStart);
				}
			}

			return res;
		}

		/// <summary>
		/// Returns a printable string containing a human readable
		/// representation of the specified data.
		/// </summary>
		/// <param name="prefix">
		/// Prefix for the string to be returned.
		/// </param>
		/// <param name="data">
		/// Data to be represented.
		/// </param>
		/// <param name="offset">
		/// Start offset within data bounds.
		/// </param>
		/// <param name="len">
		/// Length of the data to be represented.
		/// </param>
		/// <returns>
		/// A printable string with a human readable representation of
		/// the specified data.
		/// </returns>
		public static string GetPrintableBuffer( string prefix, byte[] data,
			int offset, int len) {

			if ( ( data == null) || ( data.Length == 0) || ( len == 0) ||
				( ( offset + len) > data.Length)) {
				return string.Empty;
			}

			StringBuilder s = new StringBuilder( len * 4);
			if ( !IsNullOrEmpty( prefix)) {
				s.Append( prefix);
				s.Append( Environment.NewLine);
			}

			char c;
			int i, j;
			int charsPerLine = 20;
			s.Append( "     1 |");
			for ( i = 0; i < len; i++) {
				if ( ( i > 0) && ( ( i % charsPerLine) == 0)) {
					s.Append( "| ");
					for ( j = i - charsPerLine; j < i; j++) {
						c = ( char)data[j + offset];
						c = ( char)( ( c >> 4) & 0x0F);
						if ( c < 10) {
							c += '0';
						} else {
							c += '7';
						}
						s.Append( c);
						c = ( char)( ( ( char)data[j + offset]) & 0x0F);
						if ( c < 10) {
							c += '0';
						} else {
							c += '7';
						}
						s.Append( c);
						if ( ( j + 1) < i) {
							s.Append( ' ');
						}
					}
					s.Append( Environment.NewLine);
					s.Append( string.Format( "{0,6} |", i + 1));
				}

				c = ( char)data[i + offset];
				if ( ( c != '\t') && ( char.IsLetterOrDigit( c) || char.IsPunctuation( c) ||
					char.IsSeparator( c) || char.IsSymbol( c) || char.IsWhiteSpace( c))) {
					s.Append( c);
				} else {
					s.Append( ".");
				}
			}

			s.Append( '|');
			if ( ( i % charsPerLine) == 0) {
				j = 0;
			} else {
				j = charsPerLine - ( i % charsPerLine);
			}
			for ( ; j > 0; j--) {
				s.Append( ' ');
			}
			s.Append( ' ');
			if ( ( i % charsPerLine) == 0) {
				j = i - charsPerLine;
			} else {
				j = i - ( i % charsPerLine);
			}
			for ( ; j < i; j++) {
				c = ( char)data[j + offset];
				c = ( char)( ( c >> 4) & 0x0F);
				if ( c < 10) {
					c += '0';
				} else {
					c += '7';
				}
				s.Append( c);
				c = ( char)( ( ( char)data[j + offset]) & 0x0F);
				if ( c < 10) {
					c += '0';
				} else {
					c += '7';
				}
				s.Append( c);
				if ( ( j + 1) < i) {
					s.Append( ' ');
				}
			}

			return s.ToString();
		}

		/// <summary>
		/// Returns a printable string containing a human readable
		/// representation of the specified data.
		/// </summary>
		/// <param name="prefix">
		/// Prefix for the string to be returned.
		/// </param>
		/// <param name="data">
		/// Data to be represented.
		/// </param>
		/// <returns>
		/// A printable string with a human readable representation of
		/// the specified data.
		/// </returns>
		public static string GetPrintableBuffer( string prefix, byte[] data) {

			if ( ( data == null) || ( data.Length == 0)) {
				return string.Empty;
			}

			return GetPrintableBuffer( prefix, data, 0, data.Length);
		}
		#endregion
	}
}
