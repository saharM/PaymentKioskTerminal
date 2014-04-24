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

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Define la clase base que administra e instrumenta el largo de los
	/// datos de los componentes de mensajería.
	/// </summary>
	public abstract class LengthManager {

		private int _maximumLength;

		#region Constructors
		/// <summary>
		/// Construye un nuevo administrador de largo.
		/// </summary>
		/// <param name="maximumLength">
		/// Es el largo máximo que pueden alcanzar los datos del campo.
		/// </param>
		protected LengthManager( int maximumLength) {

			_maximumLength = maximumLength;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna el largo máximo de los datos.
		/// </summary>
		public int MaximumLength {

			get {

				return _maximumLength;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Escribe el largo de los datos del campo en el contexto de
		/// formateo del mensaje.
		/// </summary>
		/// <param name="component">
		/// Es el componente de mensajería del que se está escribiendo
		/// el largo de sus datos.
		/// </param>
		/// <param name="dataLength">
		/// Es el largo de los datos del componente (puede diferir del largo
		/// de los datos obtenidos de <paramref name="component"/>, pues pueden
		/// estar rellenados)
		/// </param>
		/// <param name="encodedLength">
		/// Es el largo de los datos codificados.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo del mensaje.
		/// </param>
		public virtual void WriteLength( MessagingComponent component,
			int dataLength, int encodedLength, ref FormatterContext formatterContext) {

		}

		/// <summary>
		/// Escribe en el contexto de formateo del mensaje al final de
		/// los datos del campo, el indicador de fin del largo del campo.
		/// </summary>
		/// <param name="component">
		/// Es el componente de mensajería del que se está escribiendo
		/// el largo de sus datos.
		/// </param>
		/// <param name="dataLength">
		/// Es el largo de los datos del componente (puede diferir del largo
		/// de los datos obtenidos de <paramref name="component"/>, pues pueden
		/// estar rellenados)
		/// </param>
		/// <param name="encodedLength">
		/// Es el largo de los datos codificados.
		/// </param>
		/// <param name="formatterContext">
		/// Es el contexto de formateo del mensaje.
		/// </param>
		public virtual void WriteLengthTrailer( MessagingComponent component,
			int dataLength, int encodedLength, ref FormatterContext formatterContext) {

		}

		/// <summary>
		/// Indica si existen datos suficientes como para leer el largo
		/// de los datos desde el contexto de análisis y construcción de mensajes.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// <see langref="true"/> en caso de que existan datos suficientes
		/// como para leer el largo de los datos, <see langref="false"/> en caso
		/// contrario.
		/// </returns>
		public abstract bool EnoughData( ref ParserContext parserContext);

		/// <summary>
		/// Lee desde el contexto de análisis y construcción de mensajes,
		/// el largo de los datos.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes.
		/// </param>
		/// <returns>
		/// El largo de los datos.
		/// </returns>
		public abstract int ReadLength( ref ParserContext parserContext);

		/// <summary>
		/// Lee el indicador de fin de largo del componente de mensajería.
		/// </summary>
		/// <param name="parserContext">
		/// Es el contexto de análisis y construcción de mensajes.
		/// </param>
		public virtual void ReadLengthTrailer( ref ParserContext parserContext) {

		}
		#endregion
	}
}
