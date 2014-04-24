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
using log4net.ObjectRenderer;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Representa un componente de mensajería.
	/// </summary>
    [Serializable]
	public abstract class MessagingComponent : ICloneable, IDisposable {

		#region Constructors
		/// <summary>
		/// Crea un nuevo componente de mensajería.
		/// </summary>
		protected MessagingComponent() {

		}
		#endregion

		#region Methods
		/// <summary>
		/// Convierte a un array de bytes el valor del componente de mensajería.
		/// </summary>
		/// <returns>
		/// Un array de bytes.
		/// </returns>
		public abstract byte[] GetBytes();

		/// <summary>
		/// Convierte en una cadena de caracteres el valor del componente de mensajería.
		/// </summary>
		/// <returns>
		/// Una cadena de caracteres que representan el valor del componente de mensajería.
		/// </returns>
		public override string ToString() {

			return string.Empty;
		}

		/// <summary>
		/// Construye una copia exacta del componente de mensajería.
		/// </summary>
		/// <returns>
		/// Una copia exacta del componente de mensajería.
		/// </returns>
		public abstract object Clone();

		/// <summary>
		/// Destruye la instancia del componente de mensajería.
		/// </summary>
		public virtual void Dispose() {

		}

		/// <summary>
		/// Retorna una clase que puede representar en formato XML el componente de
		/// mensajería.
		/// </summary>
		/// <param name="renderingMap">
		/// Es un mapa con todas las clases que representan/ objetos.
		/// </param>
		/// <returns>
		/// Una clase que puede representar en formato XML el componente de mensajería.
		/// </returns>
		public abstract MessagingComponentXmlRendering XmlRendering(
			RendererMap renderingMap);

		/// <summary>
		/// Crea un nuevo componente de mensajería del tipo conocido por la subclase.
		/// </summary>
		/// <returns>
		/// Un nuevo componente de mensajería.
		/// </returns>
		public abstract MessagingComponent NewComponent();
		#endregion
	}
}
