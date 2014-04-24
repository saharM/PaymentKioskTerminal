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

using log4net;

namespace Fanap.Messaging.Iso8583 {

	/// <summary>
    /// It implements an ISO 8583 messages formatter.
	/// </summary>
	public class Iso8583MessageFormatter : BasicMessageFormatter {

		private StringFieldFormatter _mtiFormatter;
		private StringField _mtiField;

		/// <summary>
		/// We can't log values for these fields.
		/// </summary>
		private int[] _restrictedLogFields = { 2, 14, 35, 45, 52, 55};

		#region Constructors
		/// <summary>
        /// It initializes a new ISO 8583 formatter.
		/// </summary>
		public Iso8583MessageFormatter() : base() {

			_mtiFormatter = null;
			_mtiField = new StringField( -1);
		}
		#endregion

		#region Properties
		/// <summary>
        /// It returns or sets the message type identifier formatter.
		/// </summary>
		public StringFieldFormatter MessageTypeIdentifierFormatter {

			get {

				return _mtiFormatter;
			}

			set {

				_mtiFormatter = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
        /// It builds a new ISO 8583 message.
		/// </summary>
		/// <returns>
        /// A new ISO 8583 message.
		/// </returns>
		public override Message NewMessage() {

			return new Iso8583Message();
		}

		/// <summary>
        /// It indicates if the specified field number can be logged.
		/// </summary>
		/// <param name="fieldNumber">
        /// The field number to known if can logged.
		/// </param>
		/// <returns>
		/// true if the field can be logged, otherwise false.
		/// </returns>
		public override bool FieldCanBeLogged( int fieldNumber) {

			for ( int i = 0; i < _restrictedLogFields.Length; i++) {
				if ( fieldNumber == _restrictedLogFields[i]) {
					return false;
				}
			}

			return true;
		}

        /// <summary>
        /// It obfuscates card data (ISO 8583 fields 2, 14, 35 and 45)
        /// </summary>
        /// <param name="data">
        /// The card data.
        /// </param>
        /// <returns>
        /// The obfuscated data.
        /// </returns>
        /// <remarks>
        /// ObfuscateCardData( 4000000000000002 ) = ************0002
        /// ObfuscateCardData( 0805 ) = ****
        /// ObfuscateCardData( 4000000000000002=0805123456 ) = ************0002=**********
        /// ObfuscateCardData( B4000000000000002^JOHN DOE^0805123456 ) = B************0002^JOHN DOE^**********
        /// </remarks>
        protected virtual string ObfuscateCardData( string data ) {

            StringBuilder b = new StringBuilder( data.Length );

            int i = data.IndexOf( '^' );
            int j = -1;
            if ( i == -1 ) {
                // Try track 2, determine the correct field separator (valids are 'D' o '=').
                i = data.IndexOf( '=' );
                if ( i == -1 ) {
                    i = data.IndexOf( 'D' );
                }

                if ( ( i == -1 ) && ( data.Length > 11 ) ) {
                    i = data.Length;
                }
            }
            else {
                // It's track 1
                j = data.IndexOf( '^', i + 1 );
            }

            for ( int k = 0; k < data.Length; k++ ) {
                if ( ( ( k <= i ) && ( k > ( i - 5 ) ) ) ||
                    ( ( k <= j ) && ( k > i ) ) ) {
                    b.Append( data[k] );
                }
                else {
                    if ( char.IsDigit( data[k] ) ) {
                        b.Append( '*' );
                    }
                    else {
                        b.Append( data[k] );
                    }
                }
            }

            return b.ToString();
        }

        /// <summary>
        /// It returns the obfuscated field value.
        /// </summary>
        /// <param name="field">
        /// The field to be logged.
        /// </param>
        /// <returns>
        /// The data to be logged representing the obfuscated field value.
        /// </returns>
        public override string ObfuscateFieldData( Field field ) {

            string ret = string.Empty;

            if ( field.FieldNumber == 52 ) {
                ret = base.ObfuscateFieldData( field );
            }
            else {
                ret = ObfuscateCardData( field.ToString() );
            }

            return ret;
        }

		/// <summary>
		/// It formats the MTI.
		/// </summary>
		/// <param name="message">
        /// The message to be formatted.
        /// </param>
		/// <param name="formatterContext">
        /// The formatter context.
        /// </param>
		public override void BeforeFieldsFormatting( Message message,
			ref FormatterContext formatterContext) {

			_mtiField.Value = Convert.ToString( ( ( Iso8583Message)
				( message)).MessageTypeIdentifier, CultureInfo.InvariantCulture); 
			_mtiFormatter.Format( _mtiField, ref formatterContext);
		}

        /// <summary>
        /// It formats a ISO 8583 message.
        /// </summary>
        /// <param name="message">
        /// It's the message to be formatted.
        /// </param>
        /// <param name="formatterContext">
        /// It's the formatter context to be used in the format.
        /// </param>
		/// <exception cref="MessagingException">
        /// If the MTI formatter it's unknown.
		/// </exception>
		public override void Format( Message message, ref FormatterContext formatterContext) {

			if ( _mtiFormatter == null) {
				throw new MessagingException( SR.MtiFormatterRequired);
			}

			base.Format( message, ref formatterContext);
		}

		/// <summary>
		/// It parses the MTI.
		/// </summary>
		/// <param name="message">
        /// The message to be parsed.
		/// </param>
		/// <param name="parserContext">
		/// It's the parser context.
		/// </param>
		/// <returns>
		/// true if the MTI was parsed, otherwise false.
		/// </returns>
		public override bool BeforeFieldsParsing( Message message,
			ref ParserContext parserContext) {

			Field mti = _mtiFormatter.Parse( ref parserContext);

			if ( mti == null) {
				return false;
			}

			try {
				( ( Iso8583Message)( message)).MessageTypeIdentifier =
					Convert.ToInt32( mti.Value, CultureInfo.InvariantCulture);
			} catch {
				throw new MessagingException( SR.CantParseMti);
			}

			return true;
		}

		/// <summary>
        /// Parses the data in the parser context and builds a new ISO 8583 message.
		/// </summary>
		/// <param name="parserContext">
        /// It's the parser context.
		/// </param>
		/// <returns>
		/// A new ISO 8583 message if the data was parsed correctly, otherwise null.
		/// </returns>
		/// <exception cref="MessagingException">
        /// If the MTI formatter it's unknown.
		/// </exception>
		public override Message Parse( ref ParserContext parserContext) {

			if ( _mtiFormatter == null) {
				throw new MessagingException( SR.MtiFormatterRequired);
			}

			return base.Parse( ref parserContext);
		}

		/// <summary>
        /// It copies the message formatter instance data into the provided message formatter.
		/// </summary>
		/// <param name="messageFormatter">
        /// It's the message formatter where the message formatter instance data is copied.
		/// </param>
		/// <remarks>
        /// The header, the mti formatter and the fields formatters, aren't cloned,
        /// the new instance and the original shares those object instances.
		/// </remarks>
		public override void CopyTo( BasicMessageFormatter messageFormatter) {

			base.CopyTo( messageFormatter);

			if ( messageFormatter is Iso8583MessageFormatter) {
				( ( Iso8583MessageFormatter)
					(messageFormatter)).MessageHeaderFormatter = MessageHeaderFormatter;
				( ( Iso8583MessageFormatter)
					(messageFormatter)).MessageTypeIdentifierFormatter = _mtiFormatter;
			}
		}

        /// <summary>
        /// It clones the formatter instance.
        /// </summary>
        /// <remarks>
        /// The header, the mti formatter and the fields formatters, aren't cloned,
        /// the new instance and the original shares those object instances.
        /// </remarks>
        /// <returns>
        /// A new instance of the formatter.
        /// </returns>
		public override object Clone() {

			Iso8583MessageFormatter formatter = new Iso8583MessageFormatter();

			CopyTo( formatter);

			return formatter;
		}
		#endregion
	}
}
