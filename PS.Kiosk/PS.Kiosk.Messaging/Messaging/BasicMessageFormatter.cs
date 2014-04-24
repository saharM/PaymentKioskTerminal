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
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using Fanap.Utilities;

using log4net;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// It implements a simple messages formatter, that can be utilized as base
	/// for other more sophisticated.
	/// </summary>
	/// <remarks>
	/// This formatter can handle messages with all the types of fields that are
	/// implemented in the framework, string fields, raw byte fields and bitmaps.
	/// There's a special handling, updating its values depending of the presence
	/// or not of their associated fields.
	/// </remarks>
	public class BasicMessageFormatter : IMessageFormatter {

		private IMessageHeaderFormatter _headerFormatter;
		private FieldFormatterCollection _fieldsFormatters;
		private int[] _bitmaps;
		private string _description;
		private string _name;
		private ILog _logger = null;
		private byte[] _packetHeader = null;

		#region Constructors
		/// <summary>
		/// It builds a new messages formatter.
		/// </summary>
		public BasicMessageFormatter() {

			_description = string.Empty;
			_name = string.Empty;
			_headerFormatter = null;
			_bitmaps = new int[4];
			InitializeBitmapTable( _bitmaps);
			_fieldsFormatters = new FieldFormatterCollection();
			_fieldsFormatters.Added += new FieldFormatterAddedEventHandler( OnFieldsFormatterAdded);
			_fieldsFormatters.Cleared += new FieldFormatterClearedEventHandler( OnFieldsFormattersCleared);
			_fieldsFormatters.Removed += new FieldFormatterRemovedEventHandler( OnFieldsFormatterRemoved);
		}
		#endregion

		#region Properties
		/// <summary>
		/// It returns the collection of field formatters known by this instance of messages formatter.
		/// </summary>
		public FieldFormatterCollection FieldFormatters {

			get {

				return _fieldsFormatters;
			}
		}

		/// <summary>
		/// It returns or assigns the description of the messages formatter.
		/// </summary>
		public string Description {

			get {

				return _description;
			}

			set {

				_description = value;
			}
		}

		/// <summary>
		/// It returns or assigns the logger employed by the instance.
		/// </summary>
		public ILog Logger {

			get {

				if ( _logger == null) {
					_logger = LogManager.GetLogger(
						MethodBase.GetCurrentMethod().DeclaringType);
				}

				return _logger;
			}

			set {

				if ( value == null) {
					_logger = LogManager.GetLogger(
						MethodBase.GetCurrentMethod().DeclaringType);
				} else {
					_logger = value;
				}
			}
		}

		/// <summary>
		/// It returns or assigns the name of the logger that is utilized.
		/// </summary>
		public string LoggerName {

			set {

				if ( StringUtilities.IsNullOrEmpty( value)) {
					Logger = null;
				} else {
					Logger = LogManager.GetLogger( value);
				}
			}

			get {

				return this.Logger.Logger.Name;
			}
		}

		/// <summary>
		/// Returns the field formatter for the specified field number.  
		/// </summary>
		/// <remarks>
		/// If the field formatter does not exist, a null value is returned.
		/// </remarks>
		public FieldFormatter this[int fieldNumber] {

			get {

				return ( FieldFormatter)_fieldsFormatters[fieldNumber];
			}
		}

		/// <summary>
		/// It returns or assigns the message header formatter. 
		/// </summary>
		public IMessageHeaderFormatter MessageHeaderFormatter {

			get {

				return _headerFormatter;
			}

			set {

				_headerFormatter = value;
			}
		}

		/// <summary>
		/// It returns or assigns the name of messages formatter instance. 
		/// </summary>
		public string Name {

			get {

				return _name;
			}

			set {

				_name = value;
			}
		}

		/// <summary>
		/// Get or set packet header.
		/// </summary>
		public string PacketHeader {

			get {

				if ( _packetHeader == null) {
					return null;
				}

				return FrameworkEncoding.GetInstance().Encoding.GetString( _packetHeader);
			}

			set {

				if ( value == null) {
					_packetHeader = null;
				} else {
                    _packetHeader = FrameworkEncoding.GetInstance().Encoding.GetBytes( value );
				}
			}
		}

		/// <summary>
		/// Set packet header, but can be specified in hex (i.e. 840 = 383430).
		/// </summary>
		public string HexadecimalPacketHeader {

			set {

				if ( StringUtilities.IsNullOrEmpty( value)) {

					_packetHeader = null;
				} else {

					_packetHeader = new byte[( value.Length + 1) >> 1];

					// Initialize result bytes.
					for ( int i = _packetHeader.Length - 1; i >= 0; i--) {
						_packetHeader[i] = 0;
					}

					// Format data.
					for ( int i = 0; i < value.Length; i++) {

						if ( value[i] < 0x40) {
							_packetHeader[( i >> 1)] |=
								( byte)( ( ( value[i]) - 0x30) << ( ( i & 1) == 1 ? 0 : 4));
						} else {
							_packetHeader[( i >> 1)] |=
								( byte)( ( ( value[i]) - 0x37) << ( ( i & 1) == 1 ? 0 : 4));
						}
					}
				}
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// It initializes an array of bitmaps.
		/// </summary>
		/// <param name="bitmaps">
		/// It's the array to initialize.
		/// </param>
		private void InitializeBitmapTable( int[] bitmaps) {

			for ( int i = 0; i < bitmaps.Length; i++) {
				bitmaps[i] = int.MinValue;
			}
		}

		/// <summary>
		/// It handles the event fired when a field formatter is added to
		/// the collection of field formatters.
		/// </summary>
		/// <param name="sender">
		/// The object sending the event.
		/// </param>
		/// <param name="e">
		/// The event parameters.
		/// </param>
		private void OnFieldsFormatterAdded( object sender, FieldFormatterEventArgs e) {

			if ( e.FieldFormatter is BitMapFieldFormatter) {

				// Check if is in our bitmap table.
				for ( int i = _bitmaps.Length - 1; i >= 0; i--) {
					if ( _bitmaps[i] == e.FieldFormatter.FieldNumber) {
						// It's in, don't add.
						return;
					}
				}

				if ( _bitmaps[0] != int.MinValue) {

					// It's full, expand it.
					int[] bitmaps = new int[_bitmaps.Length * 2];

					InitializeBitmapTable( bitmaps);

					// Copy previous data.
					for ( int i = _bitmaps.Length - 1; i >= 0; i--) {
						bitmaps[_bitmaps.Length + i] = _bitmaps[i];
					}
					_bitmaps = bitmaps;
				}

				_bitmaps[0] = e.FieldFormatter.FieldNumber;
				Array.Sort( _bitmaps);
			}
		}

		/// <summary>
		/// It handles the event fired by the field formatters collection when
		/// all the elements are removed.
		/// </summary>
		/// <param name="sender">
		/// The object sending the event.
		/// </param>
		/// <param name="e">
		/// The event parameters.
		/// </param>
		private void OnFieldsFormattersCleared( object sender, EventArgs e) {

			InitializeBitmapTable( _bitmaps);
		}

		/// <summary>
		/// It handles the event fired when a field formatter is removed from
		/// the collection of field formatters.
		/// </summary>
		/// <param name="sender">
		/// The object sending the event.
		/// </param>
		/// <param name="e">
		/// The event parameters.
		/// </param>
		private void OnFieldsFormatterRemoved( object sender, FieldFormatterEventArgs e) {

			if ( e.FieldFormatter is BitMapFieldFormatter) {

				// Check if is in our bitmap table.
				for ( int i = _bitmaps.Length - 1; i >= 0; i--) {
					if ( _bitmaps[i] == e.FieldFormatter.FieldNumber) {
						// Located, erase it.
						_bitmaps[i] = int.MinValue;
						Array.Sort( _bitmaps);
						break;
					}
				}
			}
		}

		/// <summary>
		/// It returns an array of integers containing the numbers of the bimatps formatters
		/// known by the message formatter instance. 
		/// </summary>
		/// <returns>
		/// An array of integers containing the numbers of the bitmaps formatters
		/// known by the message formatter instance. The numbers are orderer in
		/// descendent mode.
		/// If no bitmap formatters are found, a null value is returned.
		/// </returns>
		public int[] GetBitMapFieldNumbers() {

			int found = 0;
			int[] bitmaps = null;

			// Count bitmaps.
			for ( int i = _bitmaps.Length - 1; ( i >= 0) && ( _bitmaps[i] >= 0); i--) {
				found++;
			}

			if ( found > 0) {
				bitmaps = new int[found];

				// Copy bitmaps.
				for ( int i = _bitmaps.Length - 1; ( i >= 0) && ( _bitmaps[i] >= 0); i--) {
					bitmaps[bitmaps.Length - (_bitmaps.Length - i)] = _bitmaps[i];
				}
			}

			return bitmaps;
		}

		/// <summary>
		/// Indica si el número de campo indicado puede ser enviado a la
		/// bitácora.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo del que se desea saber si puede ser
		/// registrado en la bitácora.
		/// </param>
		/// <returns>
		/// Un valor lógico igual a verdadero en caso de que puedas ser
		/// enviado a la bitácora, en caso contrario un valor lógico igual
		/// a falso.
		/// </returns>
		public virtual bool FieldCanBeLogged( int fieldNumber) {

			return true;
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
        public virtual string ObfuscateFieldData( Field field ) {

            return "__obfuscated__";
        }

		/// <summary>
		/// Invocado por <see cref="Format"/> para permitir a las clases que
		/// extienden <see cref="BasicMessageFormatter"/>, operar entre el
		/// formateo del cabezal del mensaje que está siendo formateado, y
		/// sus campos.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje que se está formateando.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo que debe está siendo empleado.
		/// </param>
		/// <remarks>
		/// Sobreescriba este método si necesita por ejemplo, agregar
		/// información en el contexto de formateo entre la información
		/// del cabezal y los campos.
		/// </remarks>
		public virtual void BeforeFieldsFormatting( Message message,
			ref FormatterContext formatterContext) {

		}

        /// <summary>
        /// It formats a message.
        /// </summary>
        /// <param name="message">
        /// It's the message to be formatted.
        /// </param>
        /// <param name="formatterContext">
        /// It's the formatter context to be used in the format.
        /// </param>
		public virtual void Format( Message message,
			ref FormatterContext formatterContext) {

			formatterContext.CurrentMessage = message;

			// Write header.
			if ( _packetHeader != null) {
				formatterContext.Write( _packetHeader);
			}

			// Format header if we have one header formatter.
			if ( _headerFormatter != null) {
				try {
					_headerFormatter.Format( message.Header, ref formatterContext);
				} catch ( Exception e) {
					throw new MessagingException( SR.CantFormatMessageHeader, e);
				}
			}

			// Allow subclasses to put information between header data and
			// fields data.
			BeforeFieldsFormatting( message, ref formatterContext);

			// If bitmaps are present in the message formatter, check if we
			// must add them to the message.
            bool atLeastOne = false;
            int firstBitmap = -1;
			for ( int i = _bitmaps.Length - 1; ( i >= 0) && ( _bitmaps[i] >= 0); i--) {

                firstBitmap = i;
				if ( message.Fields.Contains( _bitmaps[i])) {
					// Check if present field is a bitmap.
					if ( !( message[_bitmaps[i]] is BitMapField)) {
						throw new MessagingException( SR.FieldMustBeBitmap( _bitmaps[i]));
					}
                    atLeastOne = true;
				} else {
					// Get bitmap from field formatters collection.
					BitMapFieldFormatter bitmap =
						( BitMapFieldFormatter)_fieldsFormatters[_bitmaps[i]];

					// Add bitmap to message if required.
					if ( message.Fields.ContainsAtLeastOne( bitmap.LowerFieldNumber,
						bitmap.UpperFieldNumber)) {
						message.Fields.Add( new BitMapField( bitmap.FieldNumber,
							bitmap.LowerFieldNumber, bitmap.UpperFieldNumber));
                        atLeastOne = true;
					}
				}
			}
            if ( !atLeastOne && firstBitmap > -1 ) {

                // In a bitmaped message, at least the first bitmap must be present.
                BitMapFieldFormatter bitmap = ( BitMapFieldFormatter )_fieldsFormatters[_bitmaps[firstBitmap]];

                message.Fields.Add( new BitMapField( bitmap.FieldNumber,
                    bitmap.LowerFieldNumber, bitmap.UpperFieldNumber ) );
            }

			// Correct bitmap values.
			message.CorrectBitMapsValues();

			// Format fields.
			for ( int i = 0; i <= message.Fields.MaximumFieldNumber; i++) {

				if ( message.Fields.Contains( i)) {

					// If we haven't the field formatter, throw an exception.
					if ( !_fieldsFormatters.Contains( i)) {
						throw new MessagingException( SR.UnknownFieldFormatter( i));
					}

					// Set parent message.
					if ( message.Fields[i] is InnerMessageField ) {
						InnerMessageField innerMessageField = message.Fields[i] as InnerMessageField;
						if ( innerMessageField != null ) {
							Message innerMessage = innerMessageField.Value as Message;
							if ( innerMessage != null ) {
								innerMessage.Parent = message;
							}
						}
					}

					try {
						_fieldsFormatters[i].Format( message.Fields[i],
							ref formatterContext);
					} catch ( Exception e) {
						throw new MessagingException( SR.CantFormatField( i), e);
					}
				}
			}
		}

		/// <summary>
		/// Invocado por <see cref="Parse"/> para permitir a las clases que
		/// extienden <see cref="BasicMessageFormatter"/>, operar entre el
		/// análisis del cabezal del mensaje que está siendo analizado, y
		/// sus campos.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje que se está analizando.
		/// </param>
		/// <param name="parserContext">
		/// Es el contexto de analisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// Si retorna un valor lógico igual a verdadero, <see cref="Parse"/>
		/// asume que se pudo efectuar satisfactoriamente el trabajo, si
		/// retorna un valor lógico igual a false <see cref="Parse"/>
		/// entiende que el análisis ha fallado.
		/// </returns>
		/// <remarks>
		/// Sobreescriba este método si necesita por ejemplo, extraer
		/// información del contexto de analisis y construcción de mensajes,
		/// que se encuentre entre la información del cabezal y la de los
		/// campos.
		/// </remarks>
		public virtual bool BeforeFieldsParsing( Message message,
			ref ParserContext parserContext) {

			return true;
		}

		/// <summary>
        /// It parses the data contained in the parser context.
		/// </summary>
		/// <param name="parserContext">
		/// It's the context holding the information to produce a new message instance.
		/// </param>
		/// <returns>
        /// The parsed message, or a null reference if the data contained in the context
        /// is insufficient to produce a new message.
		/// </returns>
		public virtual Message Parse( ref ParserContext parserContext) {

			// Create a new message if we are parsing a new one.
			if ( parserContext.CurrentMessage == null) {
				parserContext.CurrentMessage = NewMessage();
                // The message must known its formatter for message.ToString()
                parserContext.CurrentMessage.Formatter = this;
			}

			// Remove packet header data.
			if ( ( _packetHeader != null) && !parserContext.PacketHeaderDataStripped) {
				parserContext.Consumed( _packetHeader.Length);
				parserContext.PacketHeaderDataStripped = true;
			}

			// Parse header if we have a header formatter.
			if ( _headerFormatter != null) {
				// Check if the header hasn't been parsed yet.
				if ( parserContext.CurrentMessage.Header == null) {
					try {
						parserContext.CurrentMessage.Header =
							_headerFormatter.Parse( ref parserContext);
					} catch ( Exception e) {
						throw new MessagingException( SR.CantParseMessageHeader, e);
					}

					// If null, more data is needed.
					if ( parserContext.CurrentMessage.Header == null) {
						return null;
					}

					parserContext.ResetDecodedLength();
				}
			}

			// Allow subclasses to get information between header data and
			// fields data.
			if ( !parserContext.Signaled) {
				if ( !BeforeFieldsParsing( parserContext.CurrentMessage, ref parserContext)) {
					return null;
				}
				parserContext.Signaled = true;
			}

			for( int i = parserContext.CurrentField;
				i <= _fieldsFormatters.MaximumFieldFormatterNumber; i++) {

				// If we have a bitmap use it to detect present fields,
				// otherwise parse known message formatter fields.
				if ( parserContext.CurrentBitMap != null) {

					// Check if field number is out of bitmap bounds.
					if ( i > parserContext.CurrentBitMap.UpperFieldNumber) {

						// Locate another bitmap.
						bool found = false;
						for( int j = parserContext.CurrentBitMap.FieldNumber + 1;
							j < i; j++) {
              
							if ( parserContext.CurrentMessage.Fields.Contains( j)) {
								Field field = parserContext.CurrentMessage.Fields[j];
								if ( field is BitMapField) {

									// Another bitmap found.
									parserContext.CurrentBitMap = ( BitMapField)field;
									found = true;
									break;
								}
							}
						}

						if ( !found) {
							parserContext.CurrentBitMap = null;

							// No more bitmaps, continue with posible mandatory fields
							// (start from last field covered by known bitmaps, plus one).
							i = ( (BitMapFieldFormatter)
								( _fieldsFormatters[_bitmaps[_bitmaps.Length - 1]])).UpperFieldNumber + 1;
							continue;
						}
					}

					if ( !parserContext.CurrentBitMap[i]) {
						// Save current field number in context.
						parserContext.CurrentField = i;

						// Bit is not set, field is not present in the received data.
						continue;
					}
				}

				// Save current field number in context.
				parserContext.CurrentField = i;

				if ( _fieldsFormatters.Contains( i)) {

					// Get field formatter.
					FieldFormatter fieldFormatter = _fieldsFormatters[i];

					Field field;
					try {
						// to parse field.
						field = fieldFormatter.Parse( ref parserContext);
					} catch ( Exception e) {
						throw new MessagingException( SR.CantParseField( i), e);
					}

					if ( field == null) {

						if ( Logger.IsDebugEnabled) {
							Logger.Debug( SR.MoreDataNeeded);
						}

						// More data needed to parse message.
						return null;
					} else {
						parserContext.CurrentMessage.Fields.Add( field);

						// Set parent message.
						if ( field is InnerMessageField ) {
							InnerMessageField innerMessageField = field as InnerMessageField;
							Message innerMessage = innerMessageField.Value as Message;
							if ( innerMessage != null ) {
								innerMessage.Parent = parserContext.CurrentMessage;
							}
						}

						if ( Logger.IsDebugEnabled) {
							if ( field is BitMapField) {
								Logger.Debug( SR.DecodedBitmap( field.FieldNumber, field.ToString()));
							} else {
								Logger.Debug( SR.DecodedField( field.FieldNumber, field.ToString()));
							}
						}

						parserContext.ResetDecodedLength();

						// If this is the first located bitmap, save it.
						if ( ( parserContext.CurrentBitMap == null) &&
							( field is BitMapField)) {

							parserContext.CurrentBitMap = ( BitMapField)field;
						}
					}
				} else {
					if ( parserContext.CurrentBitMap != null) {
						// A field is present in current bitmap, but message formatter
						// can't parse it because field formatter isn't present.
						throw new MessagingException( SR.UnknownFormatter( i));
					}
				}
			}

			// We have a new message, get it and initialize parsing context.
			Message parsedMessage = parserContext.CurrentMessage;

			// Reset parser context.
			parserContext.MessageHasBeenConsumed();

			return parsedMessage;
		}

		/// <summary>
		/// Crea un nuevo mensaje del tipo conocido por el formateador de
		/// mensajes.
		/// </summary>
		/// <returns>
		/// Un nuevo mensaje.
		/// </returns>
		public virtual Message NewMessage() {

			return new Message();
		}

		/// <summary>
		/// Copia el formateador sobre el que se invoca este método
		/// en otro dado.
		/// </summary>
		/// <param name="messageFormatter">
		/// Es el formateador al que se copia la información del
		/// formateador sobre el que se invoca este método.
		/// </param>
		/// <remarks>
		/// El formateador del cabezal de los mensajes y los formateadores
		/// de campo no son copiados, al formateador de mensajes dado
		/// se le asignan solo referencias, es decir, el formateador que se
		/// copia en el formateador dado comparten estos objetos.
		/// </remarks>
		public virtual void CopyTo( BasicMessageFormatter messageFormatter) {

			messageFormatter.Description = _description;
			messageFormatter.Logger = _logger;
			messageFormatter.Name = _name;
			messageFormatter.MessageHeaderFormatter = _headerFormatter;

			// TODO - Sincronizar la colección.
			lock ( _fieldsFormatters) {
				foreach( FieldFormatter fieldFormatter in _fieldsFormatters) {
					messageFormatter.FieldFormatters.Add( fieldFormatter);
				}
			}
		}

		/// <summary>
		/// Construye una copia del formateador de mensajes.
		/// </summary>
		/// <remarks>
		/// El formateador del cabezal de los mensajes y los formateadores
		/// de campo no son copiados, al nuevo formateador de mensajes
		/// se le asignan solo referencias, es decir, el formateador que se
		/// clona y el formateador clonado comparten estos objetos.
		/// </remarks>
		/// <returns>
		/// Una copia exacta del formateador de mensajes.
		/// </returns>
		public virtual object Clone() {

			BasicMessageFormatter formatter = new BasicMessageFormatter();

			CopyTo( formatter);

			return formatter;
		}

		/// <summary>
		/// Retorna una instancia del atributo <see cref="FieldAttribute"/>
		/// aplicado al objeto que el método recibe como parámetro.
		/// </summary>
		/// <param name="propertyInfo">
		/// Es el objeto del cual se extrae el atributo.
		/// </param>
		/// <returns>
		/// Una instancia válida del atributo <see cref="FieldAttribute"/>,
		/// o <see langref="null"/> en caso de que el objeto pasado como
		/// parámetro no tenga el atributo mencionado.
		/// </returns>
		private static FieldAttribute GetFieldAttribute(
			PropertyInfo propertyInfo) {

			object[] attributes = propertyInfo.GetCustomAttributes(
				typeof( FieldAttribute), false);

			if ( attributes.Length == 1) {
				return ( FieldAttribute)attributes[0];
			} else {
				return null;
			}
		}

		/// <summary>
		/// Asigna los campos del mensaje en base a las propiedades del
		/// objeto dado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje al que se asignan sus campos.
		/// </param>
		/// <param name="fieldsContainer">
		/// Es el objeto del que se obtienen los valores para los campos.
		/// </param>
		/// <remarks>
		/// Este método recorre todas las propiedades visibles de
		/// fieldsContainer, buscando aquellas a las que se le aplico el
		/// atributo <see cref="FieldAttribute"/>. El valor de las
		/// propiedades que encuentra, es asignado al valor del campo
		/// cuyo número corresponde con el indicado en el atributo
		/// mencionado.
		/// El valor es formateado empleando el formateador de valor
		/// indicado en el formateador de campo que corresponde al número
		/// del campo. Si el formateador de campo no tiene un formateador
		/// de valor, se emplea el método ToString sobre el valor de la
		/// propiedad.
		/// Para invocar este método, es necesario que el mensaje conozca
		/// su fomateador, pues de éste se obtienen los formateadores de los
		/// valores de los campos.
		/// </remarks>
		/// <exception cref="NullReferenceException">
		/// El parámetro indicado es nulo.
		/// </exception>
		public void AssignFields( Message message, object fieldsContainer) {

			if ( message == null) {
				throw new ArgumentNullException( "message");
			}

			if ( fieldsContainer == null) {
				throw new ArgumentNullException( "fieldsContainer");
			}

			foreach( PropertyInfo propertyInfo in
				fieldsContainer.GetType().GetProperties(
				BindingFlags.Public | BindingFlags.Instance)) {

				// Property must be readable and not indexed.
				if ( ( propertyInfo.CanRead) &&
					( propertyInfo.GetIndexParameters().Length == 0)) {

					// Get FieldAttribute.
					FieldAttribute fieldAttribute =
						GetFieldAttribute( propertyInfo);

					if ( fieldAttribute != null) {

						if ( _fieldsFormatters.Contains(
							fieldAttribute.FieldNumber)) {

							FieldFormatter fieldFormatter =
								_fieldsFormatters[fieldAttribute.FieldNumber];

							if ( fieldFormatter is StringFieldFormatter) {

								if ( ( ( StringFieldFormatter)
									( fieldFormatter)).ValueFormatter == null) {
									// Field formatter doesn't provide a valid value
									// formatter, default is ToString method.
									message.Fields.Add( fieldAttribute.FieldNumber,
										propertyInfo.GetValue( fieldsContainer,
										null).ToString());
								} else {
									// Format property value with value formatter.
									message.Fields.Add( fieldAttribute.FieldNumber,
										( ( StringFieldFormatter)
										( fieldFormatter)).ValueFormatter.Format(
										propertyInfo.GetValue( fieldsContainer,
										null)));
								}
							} else {
								// Field formatter isn't a StringFieldFormatter,
								// default is ToString method.
								message.Fields.Add( fieldAttribute.FieldNumber,
									propertyInfo.GetValue( fieldsContainer,
									null).ToString());
							}
						} else {
							// Field formatter doesn't include a valid field
							// value formatter, default is ToString method.
							message.Fields.Add( fieldAttribute.FieldNumber,
								propertyInfo.GetValue( fieldsContainer,
								null).ToString());
						}
					}
				}
			}
		}

		/// <summary>
		/// Trata de convertir el valor del campo a el tipo de la propiedad
		/// indicada.
		/// </summary>
		/// <param name="fieldsContainer">
		/// Es el objeto que expone la propiedad en la que se desea almacenar
		/// el valor convertido del campo.
		/// </param>
		/// <param name="propertyInfo">
		/// Es la propiedad a emplear.
		/// </param>
		/// <param name="fieldAttribute">
		/// Es el atributo asociado a la propiedad.
		/// </param>
		/// <param name="valueToConvert">
		/// Es el valor a convertir.
		/// </param>
		/// <exception cref="MessagingException">
		/// No se ha podido convertir el valor del campo, para asignarlo
		/// a la propiedad indicada.
		/// </exception>
		private void ApplyDefaultConvertion( object fieldsContainer,
			PropertyInfo propertyInfo, FieldAttribute fieldAttribute,
			object valueToConvert) {

			object convertedValue = null;

			try {
				// These are our default supported types.
				switch( propertyInfo.PropertyType.Name) {
					case "Object":
						convertedValue = valueToConvert;
						break;
					case "Boolean":
						convertedValue = Convert.ToBoolean( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Byte":
						convertedValue = Convert.ToByte( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Char":
						convertedValue = Convert.ToChar( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "DateTime":
						convertedValue = Convert.ToDateTime( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Decimal":
						convertedValue = Convert.ToDecimal( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Double":
						convertedValue = Convert.ToDouble( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Int16":
						convertedValue = Convert.ToInt16( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Int32":
						convertedValue = Convert.ToInt32( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Int64":
						convertedValue = Convert.ToInt64( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "SByte":
						convertedValue = Convert.ToSByte( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "Single":
						convertedValue = Convert.ToSingle( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "String":
						convertedValue = Convert.ToString( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "UInt16":
						convertedValue = Convert.ToUInt16( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "UInt32":
						convertedValue = Convert.ToUInt32( valueToConvert, CultureInfo.InvariantCulture);
						break;
					case "UInt64":
						convertedValue = Convert.ToUInt64( valueToConvert, CultureInfo.InvariantCulture);
						break;
				}
			} catch ( Exception e) {
				throw new MessagingException( SR.CantConvertFieldToProperty(
					fieldAttribute.FieldNumber, propertyInfo.Name), e);
			}

			if ( convertedValue != null) {
				// Set property value.
				propertyInfo.SetValue( fieldsContainer, convertedValue, null);
			}
		}

		/// <summary>
		/// Asigna a las propiedades del objeto dado, valores tomados de los
		/// valores de los campos del mensaje.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje al que se asignan sus campos.
		/// </param>
		/// <param name="fieldsContainer">
		/// Es el objeto al que se asignan sus propiedades con los valores
		/// de los campos del mensaje.
		/// </param>
		/// <remarks>
		/// Este método recorre todas las propiedades visibles de
		/// fieldsContainer, buscando aquellas a las que se le aplico el
		/// atributo <see cref="FieldAttribute"/>. El valor de las
		/// propiedades que encuentra, es asignado con el valor del campo
		/// cuyo número corresponde con el indicado en el atributo
		/// mencionado.
		/// El valor es convertido empleando el formateador de valor
		/// indicado en el formateador de campo que corresponde al número
		/// del campo.
		/// Para invocar este método, es necesario que el mensaje conozca
		/// su fomateador, pues de éste se obtienen los formateadores de los
		/// valores de los campos.
		/// </remarks>
		/// <exception cref="NullReferenceException">
		/// El mensaje no tiene asignado un formateador de mensajes.
		/// </exception>
		/// <exception cref="MessagingException">
		/// No se ha podido convertir el valor del campo, para asignarlo
		/// a la propiedad indicada.
		/// </exception>
		public void RetrieveFields( Message message, object fieldsContainer) {

			if ( message == null) {
				throw new ArgumentNullException( "message");
			}

			if ( fieldsContainer == null) {
				throw new ArgumentNullException( "fieldsContainer");
			}

			foreach( PropertyInfo propertyInfo in
				fieldsContainer.GetType().GetProperties(
				BindingFlags.Public | BindingFlags.Instance)) {

				// Property must be writable and not indexed.
				if ( ( propertyInfo.CanWrite) &&
					( propertyInfo.GetIndexParameters().Length == 0)) {

					// Get FieldAttribute.
					FieldAttribute fieldAttribute =
						GetFieldAttribute( propertyInfo);

					if ( fieldAttribute != null) {

						if ( message.Fields.Contains( fieldAttribute.FieldNumber) &&
							_fieldsFormatters.Contains( fieldAttribute.FieldNumber)) {

							FieldFormatter fieldFormatter =
								_fieldsFormatters[fieldAttribute.FieldNumber];

							if ( fieldFormatter is StringFieldFormatter) {
								if ( ( ( StringFieldFormatter)
									( fieldFormatter)).ValueFormatter == null) {
									// Field formatter doesn't provide a valid value
									// formatter, try default convertion.
									ApplyDefaultConvertion( fieldsContainer,
										propertyInfo, fieldAttribute,
										message[fieldAttribute.FieldNumber].Value);
								} else {
									propertyInfo.SetValue( fieldsContainer,
										( ( StringFieldFormatter)
										( fieldFormatter)).ValueFormatter.Parse(
										propertyInfo.PropertyType,
										message[fieldAttribute.FieldNumber].ToString()), null);
								}
							} else {
								// Field formatter isn't a StringFieldFormatter,
								// try default convertion.
								ApplyDefaultConvertion( fieldsContainer,
									propertyInfo,  fieldAttribute,
									message[fieldAttribute.FieldNumber].Value);
							}
						} else {
							// Field formatter doesn't include a valid field
							// value formatter, try default convertion.
							ApplyDefaultConvertion( fieldsContainer,
								propertyInfo,  fieldAttribute,
								message[fieldAttribute.FieldNumber].Value);
						}
					}
				}
			}
		}
		#endregion
	}
}
