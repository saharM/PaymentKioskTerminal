#region Copyright (C) 2004-2006 Diego Zabaleta, Leonardo Zabaleta
//
// Copyright � 2004-2006 Diego Zabaleta, Leonardo Zabaleta
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
using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa un secuenciador para generar n�meros entre 1 y 999999. Es ideal
	/// para ser utilizado como secuenciador de n�meros trace en mensajes de tipo
	/// ISO 8583 (STAN es la sigla para el t�rmino en ingl�s System Trace Audit Number).
	/// </summary>
    [Serializable]
	public class VolatileStanSequencer : VolatileSequencer	{

		/// <summary>
		/// Es el valor m�nimo por defecto que puede valer el secuenciador.
		/// </summary>
		public const int StanMinimumValue = 1;

		/// <summary>
		/// Es el valor m�ximo por defecto que puede valer el secuenciador.
		/// </summary>
		public const int StanMaximumValue = 999999;

		#region Constructors
		/// <summary>
		/// Crea un secuenciador ideal para emplearse como generador de n�meros para
		/// el seguimiento de mensajes ISO 8583. Generalmente este n�mero es asignado
		/// por la parte que genera el mensaje de requerimiento, y va en el campo 11.
		/// </summary>
		public VolatileStanSequencer() :
			base ( StanMinimumValue, StanMaximumValue) {

		}
		#endregion
	}
}
