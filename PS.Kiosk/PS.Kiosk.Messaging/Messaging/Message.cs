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
using System.Reflection;
using System.Text;
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Define la representación abstracta de un mensaje.
	/// </summary>
	/// <remarks>
	/// Los mensajes se intercambian entre sistemas, con formatos
	/// de datos implementados por los diversos formateadores de
	/// mensajes que ofrece en el framework.
	/// </remarks>
    [Serializable]
	public class Message : MessagingComponent {

		private MessageHeader _header;
		private FieldCollection _fields;
		private IMessageFormatter _formatter;
		private object _identifier;
		private Message _parent;

		#region Constructors
		/// <summary>
		/// Contruye un nuevo mensaje.
		/// </summary>
		public Message() : base() {

			_header = null;
			_fields = new FieldCollection();
			_identifier = null;
			_parent = null;
		}
 
		#endregion

		#region Properties
		/// <summary>
		/// Retorna o asigna el cabezal del mensaje.
		/// </summary>
		public MessageHeader Header {

			get {

				return _header;
			}

			set {

				_header = value;
			}
		}

		/// <summary>
		/// Retorna o asigna el formateador del mensaje.
		/// </summary>
		public IMessageFormatter Formatter {

			get {

				return _formatter;
			}

			set {

				_formatter = value;
			}
		}

		/// <summary>
		/// Retorna la colección de campos del mensaje.
		/// </summary>
		public FieldCollection Fields {

			get {

				return _fields;
			}
		}

		/// <summary>
		/// Retorna un campo en la colección de campos.
		/// </summary>
		/// <remarks>
		/// Si el campo no existe en la colección, un valor nulo es
		/// retornado.
		/// </remarks>
		public Field this[int fieldNumber] {

			get {

				return ( Field)_fields[fieldNumber];
			}
		}

		/// <summary>
		/// Retorna la clave que identifica al mensaje.
		/// </summary>
		public virtual object Identifier {

			get {

				return _identifier;
			}

			set {

				_identifier = value;
			}
		}

		/// <summary>
		/// It returns or sets the parent message.
		/// </summary>
		/// <remarks>
		/// This property is intended to be set by the message formatter.
		/// </remarks>
		public Message Parent {

			get {

				return _parent;
			}

			set {

				_parent = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Copia los datos del mensaje en el mensaje indicado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje al que se le copian los datos.
		/// </param>

		public virtual void CopyTo( Message message) {

			if ( _header != null) {
				message.Header = ( MessageHeader)Header.Clone();
			}

			foreach( Field field in _fields) {
				message.Fields.Add( ( Field)( field.Clone()));
			}
		}

		/// <summary>
		/// Copia los datos del mensaje y los campos específicos en el mensaje
		/// indicado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje al que se le copian los datos.
		/// </param>
		/// <param name="fieldsNumbers">
		/// Son los campos a copiar al mensaje.
		/// </param>
		public virtual void CopyTo( Message message,  int[] fieldsNumbers) {

			if ( _header != null) {
				message.Header = ( MessageHeader)Header.Clone();
			}

			for( int i = 0; i < fieldsNumbers.Length; i++) {
				if ( _fields.Contains( fieldsNumbers[i])) {
					message.Fields.Add( ( Field)
						( _fields[fieldsNumbers[i]].Clone()));
				}
			}
		}

		/// <summary>
		/// Construye una copia exacta del mensaje.
		/// </summary>
		/// <returns>
		/// Una copia exacta del mensaje.
		/// </returns>
		public override object Clone() {

			Message message = new Message();

			CopyTo( message);

			return message;
		}

		/// <summary>
		/// Convierte a un array de bytes los datos del mensaje.
		/// </summary>
		/// <returns>
		/// Un array de bytes.
		/// </returns>
		public override byte[] GetBytes() {

			byte[] data = null;

			if ( _formatter != null) {
				FormatterContext formatterContext = new FormatterContext(
					FormatterContext.DefaultBufferSize);

				_formatter.Format( this, ref formatterContext);

				data = formatterContext.GetData();
			}

			return data;
		}

		/// <summary>
		/// Retorna una clase que puede representar en formato XML el mensaje.
		/// </summary>
		/// <param name="renderingMap">
		/// Es un mapa con todas las clases que representan objetos.
		/// </param>
		/// <returns>
		/// Una clase que puede representar en formato XML el campo.
		/// </returns>
		public override MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap) {

			IObjectRenderer objectRendering = renderingMap.Get( typeof( Message));

			if ( objectRendering == null) {
				// Add renderer to map.
				objectRendering = new MessageXmlRendering();
				renderingMap.Put( typeof( Message), objectRendering);
			} else {
				if ( !( objectRendering is MessageXmlRendering)) {
					objectRendering = new MessageXmlRendering();
				}
			}

			return ( MessagingComponentXmlRendering)objectRendering;
		}

		/// <summary>
		/// Convierte en una cadena de caracteres el mensaje.
		/// </summary>
		/// <returns>
		/// Una cadena de caracteres que representan el mensaje.
		/// </returns>
		public override string ToString() {

			CorrectBitMapsValues();

			StringBuilder rendered = new StringBuilder();

			Field field;
			bool appended = false;

			if ( _header != null) {
				rendered.Append( "H:");
				rendered.Append( _header.ToString());
				appended = true;
			}

			int j = _fields.MaximumFieldNumber;
			for( int i = 0; i <= j; i++) {
				if ( ( field = _fields[i]) != null) {
					if ( appended) {
						rendered.Append( ",");
					}
					rendered.Append( i);
					rendered.Append( ":");
                    if ( ( _formatter == null ) ||
                        ( _formatter.FieldCanBeLogged( i ) ) ) {
                        if ( field is InnerMessageField ) {
                            rendered.Append( "{" );
                            rendered.Append( field.ToString() );
                            rendered.Append( "}" );
                        }
                        else {
                            rendered.Append( field.ToString() );
                        }
                    }
                    else {
                        rendered.Append( _formatter.ObfuscateFieldData( field ) );
                    }
					appended = true;
				}
			}

			return rendered.ToString();
		}

		/// <summary>
		/// Agrega o substituye al mensaje los campos del mensaje indicado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje desde el que se agregan o substituyen los campos.
		/// </param>
		/// <remarks>
		/// Los campos no son copiados, se agrega una referencia al campo
		/// del mensaje indicado.
		/// </remarks>
		public void MergeFields( Message message) {

			foreach ( Field field in message.Fields) {
				_fields.Add( field);
			}
		}

		/// <summary>
		/// Agrega o substituye al mensaje los campos del mensaje indicado.
		/// </summary>
		/// <param name="message">
		/// Es el mensaje desde el que se agregan o substituyen los campos.
		/// </param>
		/// <remarks>
		/// Los campos son copiados.
		/// </remarks>
		public void CopyFields( Message message) {

			foreach ( Field field in message.Fields) {
				_fields.Add( ( Field)field.Clone());
			}
		}

		/// <summary>
		/// Se encarga de actualizar el valor de los campos de tipo mapa de bits.
		/// </summary>
		/// <remarks>
		/// Cuando se trabaja con el mensaje agregando y eliminando campos, el
		/// sistema no actualiza los mapas de bits, debe invocarse este método
		/// para hacerlo.
		/// </remarks>
		public virtual void CorrectBitMapsValues() {

			Field field;
			BitMapField bitMap;

			if ( !_fields.Dirty) {
				return;
			}

			for( int i = 0; i < _fields.MaximumFieldNumber; i++) {

				if ( ( ( field = _fields[i]) != null) &&
					( field is BitMapField)) {

					bitMap = ( BitMapField)field;
					bitMap.Clear();
					for( int j = bitMap.LowerFieldNumber;
						( j <= bitMap.UpperFieldNumber) &&
						( j <= _fields.MaximumFieldNumber); j++) {

						bitMap[j] = _fields.Contains( j);
					}
				}
			}

			_fields.Dirty = false;
		}

		/// <summary>
		/// Crea un nuevo componente de mensajería del tipo <see cref="Message"/>.
		/// </summary>
		/// <returns>
		/// Un nuevo mensaje.
		/// </returns>
		public override MessagingComponent NewComponent() {

			return new Message();
		}
		#endregion
	}
  


}
