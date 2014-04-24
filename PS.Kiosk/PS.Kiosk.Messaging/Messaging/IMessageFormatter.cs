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
    /// It defines a messages formatter.
	/// </summary>
	public interface IMessageFormatter : ICloneable {

        /// <summary>
        /// It indicates if the specified field number can be logged.
        /// </summary>
        /// <param name="fieldNumber">
        /// The field number to known if can logged.
        /// </param>
        /// <returns>
        /// true if the field can be logged, otherwise false.
        /// </returns>
		bool FieldCanBeLogged( int fieldNumber);

        /// <summary>
        /// It returns the obfuscated field value.
        /// </summary>
        /// <param name="field">
        /// The field to be logged.
        /// </param>
        /// <returns>
        /// The data to be logged representing the obfuscated field value.
        /// </returns>
        string ObfuscateFieldData( Field field );

        /// <summary>
        /// It returns the collection of field formatters known by this instance of messages formatter.
        /// </summary>
		FieldFormatterCollection FieldFormatters {

			get;
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
		void Format( Message message, ref FormatterContext formatterContext);

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
		Message Parse( ref ParserContext parserContext);
	}
}
