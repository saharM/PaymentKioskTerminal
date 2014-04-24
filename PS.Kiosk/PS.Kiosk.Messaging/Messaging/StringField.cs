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
using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging
{

    /// <summary>
    /// Representa un componente de mensajería que es un campo de mensaje de
    /// tipo cadena de caracteres.
    /// </summary>
    [Serializable]
    public class StringField : Field
    {

        private string _value;

        #region Constructors
        /// <summary>
        /// Contruye un nuevo campo de mensaje.
        /// </summary>
        /// <param name="fieldNumber">
        /// Es el número del campo en el mensaje.
        /// </param>
        public StringField(int fieldNumber)
            : base(fieldNumber)
        {

            _value = null;
        }

        /// <summary>
        /// Contruye un nuevo campo de mensaje.
        /// </summary>
        /// <param name="fieldNumber">
        /// Es el número del campo en el mensaje.
        /// </param>
        /// <param name="value">
        /// Es el valor del nuevo campo.
        /// </param>
        public StringField(int fieldNumber, string value)
            : base(fieldNumber)
        {

            _value = value;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Retorna o asigna el valor del campo de tipo cadena de carateres.
        /// </summary>
        public string FieldValue
        {

            get
            {

                return _value;
            }

            set
            {

                _value = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el valor del campo.
        /// </summary>
        public override object Value
        {

            get
            {

                return _value;
            }

            set
            {

                if (value is string)
                {
                    _value = (string)value;
                }
                else if (value == null)
                {
                    _value = null;
                }
                else if (value is byte[])
                {
                    //if(this.FieldNumber == 44)
                       // _value = Encoding.UTF7.GetString((byte[])value);
                    //else
                        _value = FrameworkEncoding.GetInstance().Encoding.GetString((byte[])value);
                }
                else
                {
                    throw new ArgumentException(SR.CantHandleParameterType, "value");
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Convierte en una cadena de caracteres el valor del campo.
        /// </summary>
        /// <returns>
        /// Una cadena de caracteres que representan el valor del campo.
        /// </returns>
        public override string ToString()
        {

            if (_value == null)
            {
                return string.Empty;
            }
            else
            {
                return _value;
            }
        }

        /// <summary>
        /// Convierte a un array de bytes el valor del campo.
        /// </summary>
        /// <returns>
        /// Un array de bytes.
        /// </returns>
        public override byte[] GetBytes()
        {

            if (_value == null)
            {
                return null;
            }

            return FrameworkEncoding.GetInstance().Encoding.GetBytes(_value);
        }

        /// <summary>
        /// Construye una copia exacta del campo.
        /// </summary>
        /// <returns>
        /// Una copia exacta del campo.
        /// </returns>
        public override object Clone()
        {

            if (_value == null)
            {
                return new StringField(this.FieldNumber);
            }
            else
            {
                return new StringField(this.FieldNumber,
                    string.Copy(_value));
            }
        }

        /// <summary>
        /// Crea un nuevo campo de tipo string.
        /// </summary>
        /// <returns>
        /// Un nuevo campo de tipo string.
        /// </returns>
        public override MessagingComponent NewComponent()
        {

            return new StringField(FieldNumber);
        }
        #endregion
    }
}
