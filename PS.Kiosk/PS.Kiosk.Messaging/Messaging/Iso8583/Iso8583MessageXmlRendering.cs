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

namespace Fanap.Messaging.Iso8583 {

	/// <summary>
	/// A class capable to represent an ISO 8583 message in XML.
	/// </summary>
	public class Iso8583MessageXmlRendering : MessagingComponentXmlRendering {

		/// <summary>
		/// The ISO 8583 message tag.
		/// </summary>
		public const string XmlIso8583MessageTag = "iso8583message";

		/// <summary>
		/// The MTI tag.
		/// </summary>
		public const string XmlIso8583MessageTypeIdentifierAttr = "mti";

		#region Constructors
		/// <summary>
		/// It initializes a new instance of the class.
		/// </summary>
		public Iso8583MessageXmlRendering() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// It builds a XML representation of an ISO 8583 message.
		/// </summary>
		/// <param name="renderingMap">
		/// It's the renderer map (see log4net).
		/// </param>
		/// <param name="component">
		/// It's the ISO 8583 message.
		/// </param>
		/// <param name="indent">
		/// It's the indentation to be used.
		/// </param>
		/// <returns>
        /// A XML representation of an ISO 8583 message.
		/// </returns>
        /// <exception cref="ArgumentException">
        /// If the <paramref name="component"/> isn't an ISO 8583 message.
        /// </exception>
		public override string DoRender( RendererMap renderingMap,
			MessagingComponent component, string indent) {

			if ( !( component is Iso8583Message)) {
				throw new ArgumentException( SR.ComponentIsNotAnISO8583Message, "component");
			}

			Iso8583Message message = ( Iso8583Message)component;

			StringBuilder render = new StringBuilder();

			render.Append( indent);
			render.Append( "<");
			render.Append( XmlIso8583MessageTag);
			render.Append( " ");
			render.Append( XmlIso8583MessageTypeIdentifierAttr);
			render.Append( "=\"");
			render.Append( message.MessageTypeIdentifier.ToString( CultureInfo.InvariantCulture));
			render.Append( "\">");
			render.Append( Environment.NewLine);

			render.Append( ( ( Message)message).XmlRendering( renderingMap).DoRender(
				renderingMap, message, indent + "   "));

			render.Append( indent);
			render.Append( "</");
			render.Append( XmlIso8583MessageTag);
			render.Append( ">");
			render.Append( Environment.NewLine);

			return render.ToString();
		}
		#endregion
	}
}
