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
using System.Text;
using Fanap.Utilities;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging
{

    /// <summary>
    /// Es el contexto de análisis y contrucción de mensajes empleado por el framework,
    /// en él se almacena la información que se va recibiendo del canal de recepción de
    /// datos, el mensaje que se está construyendo a partir de la información recibida
    /// y el campo actual de ese mensaje que se está intentando construir, entre otras
    /// cosas.
    /// </summary>
    public struct ParserContext
    {

        /// <summary>
        /// En caso de que el buffer necesite ser ampliado, será ampliado la cantidad de
        /// bytes que esta constante indica.
        /// </summary>
        public const int DefaultBufferSize = 2048;

        private byte[] _buffer;
        private int _lowerDataBound;
        private int _upperDataBound;
        private Message _currentMessage;
        private int _currentField;
        private int _decodedLength;
        private BitMapField _currentBitMap;
        private bool _signaled;
        private bool _packetHeaderDataStripped;
        private object _payload;

        #region Constructors
        /// <summary>
        /// Construye un nuevo contexto de análisis y contrucción de mensajes.
        /// </summary>
        /// <param name="bufferSize">
        /// Es el tamaño inicial del buffer donde residirán los datos a desformatear.
        /// </param>
        /// <exception cref="ArgumentException">
        /// En caso de que <paramref name="bufferSize"/> sea menor a 1.
        /// </exception>
        public ParserContext(int bufferSize)
        {

            if (bufferSize < 1)
            {
                throw new ArgumentException(SR.MustBeGreaterThanZero, "bufferSize");
            }

            _buffer = new byte[bufferSize];
            _lowerDataBound = _upperDataBound = 0;
            _currentMessage = null;
            _currentField = 0;
            _decodedLength = int.MinValue;
            _currentBitMap = null;
            _signaled = false;
            _packetHeaderDataStripped = false;
            _payload = null;
        }
        #endregion

        #region Properties
        /// <summary>
        /// It returns or sets the current message.
        /// </summary>
        public Message CurrentMessage
        {

            get
            {

                return _currentMessage;
            }

            set
            {

                _currentMessage = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el número de campo que se está desformateando.
        /// </summary>
        public int CurrentField
        {

            get
            {

                return _currentField;
            }

            set
            {

                _currentField = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el largo del valor del campo que se está
        /// desformateando.
        /// </summary>
        public int DecodedLength
        {

            get
            {

                return _decodedLength;
            }

            set
            {

                _decodedLength = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el mapa de bits que se esta empleando para desformatear el
        /// mensaje.
        /// </summary>
        public BitMapField CurrentBitMap
        {

            get
            {

                return _currentBitMap;
            }

            set
            {

                _currentBitMap = value;
            }
        }

        /// <summary>
        /// Retorna o asigna la bandera utilitaria.
        /// </summary>
        /// <remarks>
        /// Esta bandera puede ser empleada por los formateadores de mensajes
        /// para marcar un estado por el cual ya han pasado.
        /// <see cref="BasicMessageFormatter"/> la emplea para saber que ya
        /// ha notificado a sus subclases de que ha analizado el cabezal del
        /// mensaje, esto es necesario, pues el análisis de un mensaje puede
        /// llevar varias invocaciones a la rutina que cumple esa función (en
        /// el caso de que no existan datos suficientes para finalizar el
        /// proceso).
        /// Por defecto esta propiedad contiene el valor lógico igual a falso,
        /// que es reasignado cada vez que se invoca al método
        /// <see cref="MessageHasBeenConsumed"/>.
        /// </remarks>
        public bool Signaled
        {

            get
            {

                return _signaled;
            }

            set
            {

                _signaled = value;
            }
        }

        /// <summary>
        /// Retorna o asigna la bandera que indica si se ha removido la información
        /// del cabezal del paquete.
        /// </summary>
        /// <remarks>
        /// Esta bandera puede ser empleada por los formateadores de mensajes
        /// para saber si han removido la información del cabezal del paquete.
        /// Por defecto esta propiedad contiene el valor lógico igual a falso,
        /// que es reasignado cada vez que se invoca al método
        /// <see cref="MessageHasBeenConsumed"/>.
        /// </remarks>
        public bool PacketHeaderDataStripped
        {

            get
            {

                return _packetHeaderDataStripped;
            }

            set
            {

                _packetHeaderDataStripped = value;
            }
        }

        /// <summary>
        /// Retorna o asigna la carga útil del contexto de análisis y contrucción
        /// de mensajes.
        /// </summary>
        /// <remarks>
        /// Esta propiedad actualmente no es empleada por el framework, su
        /// propósito es brindar al usuario la posibilidad de guardar información
        /// análisis y contrucción de mensajes, en las clases formateadoras de
        /// mensajes propietarias.
        /// Por defecto esta propiedad contiene un valor nulo, que es reasignado
        /// cada vez que se invoca al método <see cref="MessageHasBeenConsumed"/>.
        /// </remarks>
        public object Payload
        {

            get
            {

                return _payload;
            }

            set
            {

                _payload = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el puntero dentro del buffer que indica donde terminan los
        /// datos válidos.
        /// </summary>
        public int UpperDataBound
        {

            get
            {

                return _upperDataBound;
            }

            set
            {

                _upperDataBound = value;
            }
        }

        /// <summary>
        /// Retorna o asigna el puntero dentro del buffer que indica donde
        /// comienzan los datos válidos.
        /// </summary>
        public int LowerDataBound
        {

            get
            {

                return _lowerDataBound;
            }

            set
            {

                _lowerDataBound = value;
            }
        }

        /// <summary>
        /// Retorna el espacio disponible en el buffer donde residirán los datos
        /// a desformatear.
        /// </summary>
        public int FreeBufferSpace
        {

            get
            {

                return _buffer.Length - _upperDataBound;
            }
        }

        /// <summary>
        /// Retorna el tamaño del buffer.
        /// </summary>
        public int BufferSize
        {

            get
            {

                return _buffer.Length;
            }
        }

        /// <summary>
        /// Retorna la cantidad de bytes disponibles en el buffer de datos a
        /// desformatear.
        /// </summary>
        public int DataLength
        {

            get
            {

                return _upperDataBound - _lowerDataBound;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Retorna el buffer donde residirán los datos a desformatear.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffer()
        {

            return _buffer;
        }

        /// <summary>
        /// Aumenta el tamaño del buffer en al menos la cantidad de bytes
        /// indicada.
        /// </summary>
        /// <param name="count">
        /// Es la cantidad mínima de bytes en que al menos se agranda el
        /// buffer.
        /// </param>
        /// <remarks>
        /// Este método amplía el buffer en bloques cuyo tamaño está definido
        /// por <see cref="DefaultBufferSize"/>.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// En caso de que <paramref name="count"/> sea menor a 1.
        /// </exception>
        public void ResizeBuffer(int count)
        {

            if (count < 1)
            {
                throw new ArgumentOutOfRangeException("count", count, SR.MustBeGreaterThanZero);
            }

            // Save current buffer.
            byte[] buffer = _buffer;
            int dataLength = DataLength;

            // If we don't have enough space at the beginning of the buffer, resize it.
            // If yes, then only data relocation is needed.
            if (_lowerDataBound < count)
            {
                // Resize buffer.
                _buffer = new byte[_buffer.Length +
                    (((count % DefaultBufferSize) == 0) ? count :
                    (((count / DefaultBufferSize) + 1) * DefaultBufferSize))];
            }

            // Copy data.
            for (int i = 0; i < dataLength; i++)
            {
                _buffer[i] = buffer[_lowerDataBound + i];
            }

            // Update data pointers.
            _lowerDataBound = 0;
            _upperDataBound = dataLength;
        }

        /// <summary>
        /// Elimina los datos formateados contenidos en el buffer.
        /// </summary>
        public void Clear()
        {

            _lowerDataBound = _upperDataBound = 0;
        }

        /// <summary>
        /// Escribe en el buffer de datos a desformatear, los datos indicados.
        /// </summary>
        /// <param name="data">
        /// Son los datos a escribir en el buffer.
        /// </param>
        public void Write(string data)
        {

            Write(data, 0, data.Length);
        }

        /// <summary>
        /// Escribe en el buffer de datos formateados, los datos indicados.
        /// </summary>
        /// <param name="data">
        /// Son los datos a escribir en el buffer.
        /// </param>
        /// <param name="offset">
        /// Indica a partir desde donde se comienza a copiar la información
        /// contenida en <paramref name="data"/>.
        /// </param>
        /// <param name="count">
        /// Indica la cantidad de caracteres a copiar.
        /// </param>
        public void Write(string data, int offset, int count)
        {

            if (offset > data.Length)
            {
                throw new ArgumentException(SR.OutOfBound, "offset");
            }

            if ((offset + count) > data.Length)
            {
                throw new ArgumentException(SR.OutOfBound, "count");
            }

            // Check if we must resize our buffer.
            if (FreeBufferSpace < count)
            {
                ResizeBuffer(count);
            }

            // Write data.
            for (int i = 0; i < count; i++)
            {
                _buffer[_upperDataBound + i] = (byte)(data[offset + i]);
            }

            // Update upper data bound.
            _upperDataBound += count;
        }

        /// <summary>
        /// Escribe en el buffer de datos a desformatear, los datos indicados.
        /// </summary>
        /// <param name="data">
        /// Son los datos a escribir en el buffer.
        /// </param>
        public void Write(byte[] data)
        {

            Write(data, 0, data.Length);
        }

        /// <summary>
        /// Escribe en el buffer de datos a desformatear, los datos indicados.
        /// </summary>
        /// <param name="data">
        /// Son los datos a escribir en el buffer.
        /// </param>
        /// <param name="offset">
        /// Indica a partir desde donde se comienza a copiar la información
        /// contenida en <paramref name="data"/>.
        /// </param>
        /// <param name="count">
        /// Indica la cantidad de bytes a copiar.
        /// </param>
        public void Write(byte[] data, int offset, int count)
        {

            if (offset > data.Length)
            {
                throw new ArgumentException(SR.OutOfBound, "offset");
            }

            if ((offset + count) > data.Length)
            {
                throw new ArgumentException(SR.OutOfBound, "count");
            }

            // Check if we must resize our buffer.
            if (FreeBufferSpace < count)
            {
                ResizeBuffer(count);
            }

            // Write data.
            for (int i = 0; i < count; i++)
            {
                _buffer[_upperDataBound + i] = data[offset + i];
            }

            // Update upper data bound.
            _upperDataBound += count;
        }

        /// <summary>
        /// Devuelve un array de bytes conteniendo una copia de los datos
        /// almacenados en el buffer.
        /// </summary>
        /// <param name="consume">
        /// Indica en <see langref="true"/> que los datos deben ser eliminados
        /// del buffer.
        /// </param>
        /// <returns>
        /// Una copia de los datos almacenados en el buffer.
        /// </returns>
        /// <remarks>
        /// Si el buffer no contiene datos, esta función retorna
        /// <see langref="null"/>.
        /// </remarks>
        public byte[] GetData(bool consume)
        {

            if (DataLength == 0)
            {
                return null;
            }

            return GetData(consume, DataLength);
        }

        /// <summary>
        /// Devuelve un array de bytes conteniendo una copia de los datos
        /// almacenados en el buffer.
        /// </summary>
        /// <param name="consume">
        /// Indica en <see langref="true"/> que los datos deben ser eliminados
        /// del buffer.
        /// </param>
        /// <param name="count">
        /// Indica la cantidad de bytes a extraer del buffer.
        /// </param>
        /// <returns>
        /// Una copia de los datos almacenados en el buffer.
        /// </returns>
        /// <remarks>
        /// Si <paramref name="count"/> es igual a cero esta función retorna
        /// <see langref="null"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// En caso de que no exista en el buffer la cantidad de bytes
        /// especificados en el parámetro <paramref name="count"/>.
        /// </exception>
        public byte[] GetData(bool consume, int count)
        {

            if (count == 0)
            {
                return null;
            }

            if (DataLength < count)
            {
                throw new ArgumentException(SR.InsufficientData, "count");
            }

            byte[] data = new byte[count];

            for (int i = (_lowerDataBound + count - 1); i >= _lowerDataBound; i--)
            {
                data[i - _lowerDataBound] = _buffer[i];
            }

            if (consume)
            {
                Consumed(count);
            }

            return data;
        }

        /// <summary>
        /// Devuelve una cadena de caracteres con una copia de los datos
        /// almacenados en el buffer.
        /// </summary>
        /// <param name="consume">
        /// Indica en <see langref="true"/> que los datos deben ser eliminados
        /// del buffer.
        /// </param>
        /// <returns>
        /// Una copia de los datos almacenados en el buffer.
        /// </returns>
        /// <remarks>
        /// Si el buffer no contiene datos, esta función retorna
        /// <see langref="null"/>.
        /// </remarks>
        public string GetDataAsString(bool consume)
        {

            if (DataLength == 0)
            {
                return null;
            }

            return GetDataAsString(consume, DataLength);
        }

        /// <summary>
        /// Devuelve una cadena de caracteres con una copia de los datos
        /// almacenados en el buffer.
        /// </summary>
        /// <param name="consume">
        /// Indica en <see langref="true"/> que los datos deben ser eliminados
        /// del buffer.
        /// </param>
        /// <param name="count">
        /// Indica la cantidad de caracteres a extraer del buffer.
        /// </param>
        /// <returns>
        /// Una copia de los datos almacenados en el buffer.
        /// </returns>
        /// <remarks>
        /// Si <paramref name="count"/> es igual a cero esta función retorna
        /// <see langref="null"/>.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// En caso de que no exista en el buffer la cantidad de caracteres
        /// especificados en el parámetro <paramref name="count"/>.
        /// </exception>
        public string GetDataAsString(bool consume, int count)
        {

            if (count == 0)
            {
                return null;
            }

            if (DataLength < count)
            {
                throw new ArgumentException(SR.InsufficientData, "count");
            }

            //string data = FrameworkEncoding.GetInstance().Encoding.GetString(
            //    _buffer, _lowerDataBound, count);
            string data;
           
                data = FrameworkEncoding.GetInstance().Encoding.GetString(
                    _buffer, _lowerDataBound, count);
            if (_currentField == 121)
            {
                if (_currentMessage.Fields.Contains(3))
                    if (_currentMessage.Fields[3].ToString() == "390000")
                        data = System.Text.Encoding.Default.GetString(_buffer, _lowerDataBound, count);
            }

            if (consume)
            {
                Consumed(count);
            }

            return data;
        }

        /// <summary>
        /// Este método le indica al contexto de análisis y contrucción de mensajes
        /// que se reinicie el largo decodificado.
        /// </summary>
        public void ResetDecodedLength()
        {

            _decodedLength = int.MinValue;
        }

        /// <summary>
        /// Este método le indica al contexto de análisis y contrucción de mensajes
        /// que se ha consumido el mensaje que se estaba desformateando.
        /// </summary>
        public void MessageHasBeenConsumed()
        {

            _currentMessage = null;
            _currentField = 0;
            _currentBitMap = null;
            _signaled = false;
            _packetHeaderDataStripped = false;
            _payload = null;
        }

        /// <summary>
        /// Este método le indica al contexto de análisis y contrucción de mensajes
        /// que se ha consumido una cantidad específica de información desde el buffer
        /// donde residen los datos a desformatear.
        /// </summary>
        /// <param name="count">
        /// Es la cantidad de bytes consumidos desde el buffer.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Cuando la cantidad indicada de bytes consumidos supera el tamaño del
        /// buffer.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Cuando la cantidad indicada de bytes consumidos es menor a 0 o supera
        /// la cantidad de datos disponibles.
        /// </exception>
        public void Consumed(int count)
        {

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count", count,
                    SR.CantBeNegative);
            }

            _lowerDataBound += count;

            if (_lowerDataBound > _upperDataBound)
            {
                _lowerDataBound -= count;
                throw new ArgumentOutOfRangeException("count", count,
                    SR.InvalidDataConsumed);
            }

            if (_lowerDataBound == _upperDataBound)
            {
                _lowerDataBound = _upperDataBound = 0;
            }
        }

        /// <summary>
        /// Initializes the parser.
        /// </summary>
        public void Initialize()
        {

            Clear();
            ResetDecodedLength();
            MessageHasBeenConsumed();
        }

        /// <summary>
        /// Returns the string representation of the parser.
        /// </summary>
        /// <returns>
        /// An string containing internal information about the parser.
        /// </returns>
        public override string ToString()
        {

            StringBuilder res = new StringBuilder();

            res.Append(string.Format(
                "ParserContext( buffer size = {0}, lower data = {1}, upper data = {2}, ",
                _buffer.Length, _lowerDataBound, _upperDataBound));

            res.Append(string.Format(
                "current message = {0}, current field = {1}, decoded length = {2}, ",
                _currentMessage == null ? "null" : _currentMessage.GetType().ToString(),
                _currentField, _decodedLength));

            res.Append(string.Format(
                "current bitmap = {0}, signaled = {1}, ",
                _currentBitMap == null ? "null" : _currentBitMap.GetType().ToString(),
                _signaled));

            res.Append(string.Format(
                "payload = {0}, packet header data stripped = {1})",
                _payload == null ? "null" : _currentBitMap.GetType().ToString(),
                _packetHeaderDataStripped));

            if (_upperDataBound - _lowerDataBound > 0)
            {

                res.Append(Environment.NewLine);

                res.Append(StringUtilities.GetPrintableBuffer("Data in buffer:",
                    _buffer, _lowerDataBound, _upperDataBound - _lowerDataBound));
            }

            return res.ToString();
        }

        #endregion
    }
}
