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
using System.Globalization;
using System.Text;
using log4net.ObjectRenderer;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
namespace Fanap.Messaging.Iso8583 {

	/// <summary>
    /// It defines an ISO 8583 message.
	/// </summary>
    [Serializable]
    public class Iso8583Message : Message, ISerializable
    {

		private int _mti;
        private static  Iso8583Message theOneObject;

		#region Constructors
		/// <summary>
        /// It initializes a new ISO 8583 message.
		/// </summary>
		public Iso8583Message() : base() {

			_mti = int.MinValue;
		}

		/// <summary>
        /// It initializes a new ISO 8583 message.
		/// </summary>
		/// <param name="messageTypeIdentifier">
        /// It's the message type identifier.
		/// </param>
		public Iso8583Message( int messageTypeIdentifier) : base() {

			_mti = messageTypeIdentifier;
		}
        public void SetSingleton(Iso8583Message me)
        {
            theOneObject = (Iso8583Message)me.Clone();
        }
        public static Iso8583Message GetSingleton()
        {
            return theOneObject;
        }

		#endregion

		#region Properties
		/// <summary>
        /// It returns or sets the message type identifier.
		/// </summary>
		public int MessageTypeIdentifier {

			get {

				return _mti;
			}

			set {

				_mti = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
        /// It checks the MTI to inform if the message it's a request.
		/// </summary>
		/// <returns>
        /// true if the message is a request, otherwise false.
		/// </returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        void ISerializable.GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            // Instead of serializing this object, 
            // serialize a SingletonSerializationHelp instead.
            info.SetType(typeof(SingletonSerializationHelper));
            // No other values need to be added.
        }

        public bool IsRequest()
        {

			return ( ( _mti / 10) % 2) == 0;
		}

		/// <summary>
        /// It checks the MTI to inform if the message it's an advice.
		/// </summary>
		/// <returns>
        /// true if the message is an advice, otherwise false.
		/// </returns>
		public bool IsAdvice() {

			return ( ( ( _mti % 100) / 20) == 1) || ( ( ( _mti % 100) / 40) == 1);
		}

		/// <summary>
        /// If the message is a request, the MTI is changed to be a response.
		/// </summary>
		/// <exception cref="MessagingException">
		/// If the message isn't a request.
		/// </exception>
		public void SetResponseMessageTypeIdentifier() {

			if ( !IsRequest()) {
				throw new MessagingException( SR.CantSetMti);
			}

			if ( ( _mti % 2) == 1) {
				_mti--;
			}

			_mti += 10;
		}

		/// <summary>
        /// It checks the MTI to inform if the message it's an authorization.
		/// </summary>
		/// <returns>
        /// true if the message is an authorization, otherwise false.
		/// </returns>
		public bool IsAuthorization() {

			return ( ( ( _mti % 1000) / 100) == 1) && ( ( ( _mti % 1000) % 100) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a financial message.
        /// </summary>
        /// <returns>
        /// true if the message is financial, otherwise false.
        /// </returns>
		public bool IsFinancial() {

			return ( ( ( _mti % 1000) / 200) == 1) && ( ( ( _mti % 1000) % 200) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a file action message.
        /// </summary>
        /// <returns>
        /// true if the message a file action message, otherwise false.
        /// </returns>
		public bool IsFileAction() {

			return ( ( ( _mti % 1000) / 300) == 1) && ( ( ( _mti % 1000) % 300) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a reversal or chargeback.
        /// </summary>
        /// <returns>
        /// true if the message a reversal or chargeback, otherwise false.
        /// </returns>
		public bool IsReversalOrChargeBack() {

			return ( ( ( _mti % 1000) / 400) == 1) && ( ( ( _mti % 1000) % 400) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a reconciliation.
        /// </summary>
        /// <returns>
        /// true if the message a reconciliation, otherwise false.
        /// </returns>
		public bool IsReconciliation() {

			return ( ( ( _mti % 1000) / 500) == 1)  && ( ( ( _mti % 1000) % 500) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's an administrative message.
        /// </summary>
        /// <returns>
        /// true if the message an administrative message, otherwise false.
        /// </returns>
		public bool IsAdministrative() {

			return ( ( ( _mti % 1000) / 600) == 1) && ( ( ( _mti % 1000) % 600) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a fee collection message.
        /// </summary>
        /// <returns>
        /// true if the message a fee collection message, otherwise false.
        /// </returns>
		public bool IsFeeCollection() {

			return ( ( ( _mti % 1000) / 700) == 1) && ( ( ( _mti % 1000) % 700) < 100);
		}

        /// <summary>
        /// It checks the MTI to inform if the message it's a network management message.
        /// </summary>
        /// <returns>
        /// true if the message a network management message, otherwise false.
        /// </returns>
		public bool IsNetworkManagement() {

			return ( ( ( _mti % 1000) / 800) == 1) && ( ( ( _mti % 1000) % 800) < 100);
		}

		/// <summary>
		/// It returns a string representation of the ISO 8583 message.
		/// </summary>
		/// <returns>
        /// A string representation of the ISO 8583 message.
		/// </returns>
		public override string ToString() {

			CorrectBitMapsValues();

			StringBuilder rendered = new StringBuilder();

			Field field;

			if ( Header != null) {
				rendered.Append( "H:");
				rendered.Append( Header.ToString());
				rendered.Append( ",");
			}

			rendered.Append( "M:");
			rendered.Append( _mti.ToString( CultureInfo.InvariantCulture));

			int j = Fields.MaximumFieldNumber;
			for( int i = 0; i <= j; i++) {
				if ( ( field = Fields[i]) != null) {
					rendered.Append( ",");
					rendered.Append( i);
					rendered.Append( ":");
                    if ( ( Formatter == null ) ||
                        ( Formatter.FieldCanBeLogged( i ) ) ) {
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
                        rendered.Append( Formatter.ObfuscateFieldData( field ) );
                    }
				}
			}

			return rendered.ToString();
		}

		/// <summary>
		/// It clones the ISO 8583 message instance.
		/// </summary>
		/// <returns>
		/// A clone of the message instance.
		/// </returns>
		public override object Clone() {

			Iso8583Message clon = new Iso8583Message( _mti);
			CopyTo( clon);

			return clon;
		}

		/// <summary>
        /// It copies the message instance data into the provided message.
		/// </summary>
		/// <param name="message">
		/// It's the message where the message instance data is copied.
		/// </param>
		public override void CopyTo( Message message) {

			if ( message is Iso8583Message) {
				( ( Iso8583Message)message).MessageTypeIdentifier = _mti;
			}

			base.CopyTo( message);
		}

		/// <summary>
        /// It copies the message instance data and the specified fields into
        /// the provided message.
		/// </summary>
		/// <param name="message">
        /// It's the message where the message instance data is copied.
		/// </param>
		/// <param name="fieldsNumbers">
		/// The fields numbers to be copied into the provided message.
		/// </param>
		public override void CopyTo( Message message, int[] fieldsNumbers) {

			if ( message is Iso8583Message) {
				( ( Iso8583Message)message).MessageTypeIdentifier = _mti;
			}

			base.CopyTo( message, fieldsNumbers);
		}

		/// <summary>
        /// It returns a class capable to represent an ISO 8583 message
        /// in XML format.
		/// </summary>
		/// <param name="renderingMap">
        /// It's a map containing the renderers known by the system.
		/// </param>
		/// <returns>
        /// A class capable to represent an ISO 8583 message
        /// in XML format.
		/// </returns>
		public override MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap) {

			IObjectRenderer objectRendering = renderingMap.Get( typeof( Iso8583Message));

			if ( objectRendering == null) {
				// Add renderer to map.
				objectRendering = new Iso8583MessageXmlRendering();
				renderingMap.Put( typeof( Iso8583Message), objectRendering);
			} else {
				if ( !( objectRendering is MessageXmlRendering)) {
					objectRendering = new MessageXmlRendering();
				}
			}

			return ( MessagingComponentXmlRendering)objectRendering;
		}

		/// <summary>
        /// It builds a new component of <see cref="Iso8583Message"/> type.
		/// </summary>
		/// <returns>
		/// A new ISO 8583 message.
		/// </returns>
		public override MessagingComponent NewComponent() {

			return new Iso8583Message();
		}
		#endregion
	}
    [Serializable]
    internal sealed class SingletonSerializationHelper : IObjectReference
    {
        // This object has no fields (although it could).

        // GetRealObject is called after this object is deserialized.
        public Object GetRealObject(StreamingContext context)
        {
            // When deserialiing this object, return a reference to 
            // the Singleton object instead.
            return Iso8583Message.GetSingleton();
        }
    }
}
