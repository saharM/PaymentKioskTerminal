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

namespace Fanap.Messaging {

	/// <summary>
    /// Implements a binary fields formatter.
	/// </summary>
	public class BinaryFieldFormatter : FieldFormatter {

		private IBinaryEncoder _encoder;
		private LengthManager _lengthManager;

		#region Constructors
		/// <summary>
		/// It initializes a new binary field formatter instance.
		/// </summary>
		/// <param name="fieldNumber">
        /// It's the number of the field this formatter formats/parse.
		/// </param>
        /// <param name="lengthManager">
        /// It's the field length manager.
		/// </param>
		/// <param name="encoder">
        /// It's the field value encoder.
		/// </param>
		public BinaryFieldFormatter( int fieldNumber, LengthManager lengthManager,
            IBinaryEncoder encoder )
            : this( fieldNumber, lengthManager, encoder, string.Empty) {

		}

		/// <summary>
        /// It initializes a new binary field formatter instance.
		/// </summary>
		/// <param name="fieldNumber">
        /// It's the number of the field this formatter formats/parse.
		/// </param>
        /// <param name="lengthManager">
        /// It's the field length manager.
		/// </param>
		/// <param name="encoder">
        /// It's the field value encoder.
		/// </param>
		/// <param name="description">
        /// It's the description of the field formatter.
		/// </param>
        public BinaryFieldFormatter( int fieldNumber, LengthManager lengthManager,
			IBinaryEncoder encoder, string description) :
			base( fieldNumber, description) {

            if ( lengthManager == null ) {
                throw new ArgumentNullException( "lengthManager" );
			}

			if ( encoder == null) {
				throw new ArgumentNullException( "encoder");
			}

            _lengthManager = lengthManager;
			_encoder = encoder;
		}
		#endregion

		#region Properties
		/// <summary>
        /// It returns the field length manager.
		/// </summary>
		public LengthManager LengthManager {

			get {

				return _lengthManager;
			}
		}

		/// <summary>
        /// It returns the field value encoder.
		/// </summary>
		public IBinaryEncoder Encoder {

			get {

				return _encoder;
			}
		}
		#endregion

		#region Methods
        /// <summary>
        /// Formats the specified field.
        /// </summary>
        /// <param name="field">
        /// It's the field to format.
        /// </param>
        /// <param name="formatterContext">
        /// It's the context of formatting to be used by the method.
        /// </param>
		public override void Format( Field field,
			ref FormatterContext formatterContext) {

			if ( !( field is BinaryField)) {
				throw new ArgumentException(
					SR.BinaryMessageFieldExpected, "field");
			}

            if ( ( field == null ) | ( field.GetBytes() == null ) ) {
				_lengthManager.WriteLength( field, 0, 0, ref formatterContext);
				_lengthManager.WriteLengthTrailer( field, 0, 0, ref formatterContext);
			} else {
				_lengthManager.WriteLength( field, field.GetBytes().Length,
					_encoder.GetEncodedLength( field.GetBytes().Length),
					ref formatterContext);
				_encoder.Encode( field.GetBytes(), ref formatterContext);
				_lengthManager.WriteLengthTrailer( field, field.GetBytes().Length,
					_encoder.GetEncodedLength( field.GetBytes().Length),
					ref formatterContext);
			}
		}

        /// <summary>
        /// It parses the information in the parser context and builds the field.
        /// </summary>
        /// <param name="parserContext">
        /// It's the parser context.
        /// </param>
        /// <returns>
        /// The new field built with the information found in the parser context.
        /// </returns>
		public override Field Parse( ref ParserContext parserContext) {

			// If MinValue, at this moment the length hasn't been decoded.
			if ( parserContext.DecodedLength == int.MinValue) {
				if ( !_lengthManager.EnoughData( ref parserContext)) {
					// Insufficient data to parse length, return null.
					return null;
				}

				// Save length in parser context just in case field value
				// can't be parsed at this time (more data needed).
				parserContext.DecodedLength =
					_lengthManager.ReadLength( ref parserContext);
			}

			if ( parserContext.DataLength < _encoder.GetEncodedLength(
				parserContext.DecodedLength)) {

				// Insufficient data to parse field value, return null.
				return null;
			}

			// Create the new messaging component with parsing context data.
			BinaryField field = new BinaryField( FieldNumber,
				_encoder.Decode( ref parserContext,
				parserContext.DecodedLength));

			_lengthManager.ReadLengthTrailer( ref parserContext);

			return field;
		}
		#endregion
	}
}
