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
using System.Collections;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa una colecci�n de campos.
	/// </summary>
	/// <remarks>
	/// El n�mero de campo es empleado como clave dentro de la colecci�n.
	/// </remarks>
    [Serializable]
	public class FieldCollection : IEnumerable {

		private Hashtable _fields;
		private int _maxField;
		private bool _dirty;
		private bool _maxFieldDirty;

		#region Constructors
		/// <summary>
		/// Crea una nueva instancia de la colecci�n de campos.
		/// </summary>
		public FieldCollection() {

			_fields = new Hashtable( 64);
			_maxField = int.MinValue;
			_dirty = false;
			_maxFieldDirty = false;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Retorna un campo en la colecci�n de campos.
		/// </summary>
		/// <remarks>
		/// Si el campo no existe en la colecci�n, un valor nulo es
		/// retornado.
		/// </remarks>
		public Field this[int fieldNumber] {

			get {

				return ( Field)_fields[fieldNumber];
			}
		}

		/// <summary>
		/// Retorna la cantidad de campos incluidos en la colecci�n.
		/// </summary>
		public int Count {

			get {

				return _fields.Count;
			}
		}

		/// <summary>
		/// Retorna el n�mero del campo cuyo n�mero sea el mayor contenido en
		/// la colecci�n.
		/// </summary>
		/// <exception cref="ApplicationException">
		/// La colecci�n se encuentra vac�a.
		/// </exception>
		public int MaximumFieldNumber {

			get {

				if ( _fields.Count == 0) {
					throw new ApplicationException( SR.TheCollectionIsEmpty);
				}

				if ( _maxFieldDirty) {

					_maxField = int.MinValue;

					foreach( DictionaryEntry field in _fields) {
						if ( ( ( Field)( field.Value)).FieldNumber > _maxField) {
							_maxField = ( ( Field)( field.Value)).FieldNumber;
						}
					}
					_maxFieldDirty = false;
				}

				return _maxField;
			}
		}

		/// <summary>
		/// Asigna o devuelve un valor de tipo l�gico que indica si la colecci�n
		/// ha sufrido modificaciones.
		/// </summary>
		/// <remarks>
		/// Esta propiedad puede ser empleada por el usuario para seguir
		/// la pista de la colecci�n, cuando se agregan o borran campos a la
		/// colecci�n esta propiedad pasa a valer <see langref="true"/>, es
		/// responsabilidad del usuario asignarla en <see langref="true"/> cuando
		/// lo jusgue necesario.
		/// </remarks>
		public bool Dirty {

			get {

				return _dirty;
			}

			set {

				_dirty = value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Agrega un campo a la colecci�n.
		/// </summary>
		/// <param name="field">
		/// Es el campo a agregar a la colecci�n.
		/// </param>
		/// <remarks>
		/// Si existe es reemplazado.
		/// </remarks>
		public void Add( Field field) {

			if ( field == null) {
				return;
			}

			_fields[field.FieldNumber] = field;

			if ( _maxField < field.FieldNumber) {
				_maxField = field.FieldNumber;
			}

			_dirty = true;
		}

		/// <summary>
		/// Agrega a la colecci�n un campo cuyo valor es una cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el n�mero del campo a agregar a colecci�n.
		/// </param>
		/// <param name="fieldValue">
		/// Es el valor del campo a agregar.
		/// </param>
		/// <remarks>
		/// Si existe es reemplazado.
		/// </remarks>
		public void Add( int fieldNumber, string fieldValue) {

			Add( new StringField( fieldNumber, fieldValue));
		}

		/// <summary>
		/// Agrega a la colecci�n un campo cuyo valor es un array de bytes.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el n�mero del campo a agregar a colecci�n.
		/// </param>
		/// <param name="fieldValue">
		/// Es el valor del campo a agregar.
		/// </param>
		/// <remarks>
		/// Si existe es reemplazado.
		/// </remarks>
		public void Add( int fieldNumber, byte[] fieldValue) {

			Add( new BinaryField( fieldNumber, fieldValue));
		}

        /// <summary>
        /// Agrega a la colecci�n un campo cuyo valor es un mensaje.
        /// </summary>
        /// <param name="fieldNumber">
        /// Es el n�mero del campo a agregar a colecci�n.
        /// </param>
        /// <param name="fieldValue">
        /// Es el valor del campo a agregar.
        /// </param>
        /// <remarks>
        /// Si existe es reemplazado.
        /// </remarks>
        public void Add( int fieldNumber, Message fieldValue ) {

            Add( new InnerMessageField( fieldNumber, fieldValue ) );
        }

		/// <summary>
		/// Elimina el campo cuyo n�mero coincida con el especificado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el n�mero del campo que se desea eliminar de la colecci�n.
		/// </param>
		public void Remove( int fieldNumber) {

			_fields.Remove( fieldNumber);

			_dirty = true;

			if ( fieldNumber == _maxField) {
				_maxFieldDirty = true;
			}
		}

		/// <summary>
		/// Elimina los campos cuyos n�meros coincidan con los indicados.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Es el conjunto de n�meros de campos a eliminar de la colecci�n.
		/// </param>
		public void Remove( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				Remove( fieldsNumbers[i]);
			}
		}

		/// <summary>
		/// Elimina todos los campos de la colecci�n.
		/// </summary>
		public void Clear() {

			if ( _fields.Count == 0) {
				return;
			}

			_fields.Clear();
			_dirty = true;
			_maxField = int.MinValue;
		}

		/// <summary>
		/// Indica si la colecci�n contiene un campo para el n�mero de campo
		/// indicado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el n�mero del campo para el que se desea conocer si existe un
		/// campo en la colecci�n.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si el campo est� contenido en la colecci�n,
		/// <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( int fieldNumber) {

			return _fields.Contains( fieldNumber);
		}

		/// <summary>
		/// Indica si la colecci�n contiene todos los campos cuyos n�meros se
		/// indican.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Son los n�meros de campos para los que se desea conocer si existen
		/// en la colecci�n.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si todos los campos est�n contenidos en la
		/// colecci�n, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				if ( !Contains( fieldsNumbers[i])) {
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Indica si la colecci�n contiene por lo menos uno de los campos
		/// cuyos n�meros se indican.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Son los n�meros de campos para los que se desea conocer si existe
		/// al menos uno en la colecci�n.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si uno de los campos est� contenidos en la
		/// colecci�n, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool ContainsAtLeastOne( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				if ( Contains( fieldsNumbers[i])) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Indica si la colecci�n contiene por lo menos uno de los campos
		/// cuyos n�meros se encuentran en un rango dado.
		/// </summary>
		/// <param name="lowerFieldNumber">
		/// Es el n�mero inicial del rango.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el n�mero final del rango.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si uno de los campos est� contenidos en la
		/// colecci�n, <see langref="false"/> en caso contrario.
		/// </returns>
		public bool ContainsAtLeastOne( int lowerFieldNumber, int upperFieldNumber) {

			for ( int i = lowerFieldNumber; i <= upperFieldNumber; i++) {
				if ( Contains( i)) {
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Cambia el n�mero de un campo por otro dado.
		/// </summary>
		/// <param name="oldFieldNumber">
		/// Es el n�mero de campo que se desea cambiar.
		/// </param>
		/// <param name="newFieldNumber">
		/// Es el nuevo n�mero de campo.
		/// </param>
		/// <exception cref="ArgumentException">
		/// El campo no existe.
		/// </exception>
		public void MoveField( int oldFieldNumber, int newFieldNumber) {

			if ( !_fields.Contains( oldFieldNumber)) {
				throw new ArgumentException( SR.FieldDoesntExists, "oldFieldNumber");
			}

			Field field = this[oldFieldNumber];
			Remove( oldFieldNumber);
			Field newField = ( Field)field.NewComponent();
			newField.SetFieldNumber( newFieldNumber);
			newField.Value = field.Value;
			Add( newField);
		}
		#endregion

		#region Implementation of IEnumerable
		/// <summary>
		/// Devuelve un enumerador de la colecci�n.
		/// </summary>
		/// <returns>
		/// El enumerador sobre la colecci�n.
		/// </returns>
		public IEnumerator GetEnumerator() {

			return new FieldsEnumerator( _fields);
		}

		/// <summary>
		/// Implementa el enumerador de la colecci�n.
		/// </summary>
		private class FieldsEnumerator : IEnumerator {

			private IEnumerator _fieldsEnumerator;

			#region Constructors
			/// <summary>
			/// Crea una nueva instancia de la clase <see cref="FieldsEnumerator"/>.
			/// </summary>
			/// <param name="fields">
			/// Es la tabla de hash que contiene los campos.
			/// </param>
			public FieldsEnumerator( Hashtable fields) {

				_fieldsEnumerator = fields.GetEnumerator();
			}
			#endregion

			#region Implementation of IEnumerator
			/// <summary>
			/// Reinicia la enumeraci�n.
			/// </summary>
			public void Reset() {
			
				_fieldsEnumerator.Reset();
			}

			/// <summary>
			/// Se mueve al siguiente elemento en la enumeraci�n.
			/// </summary>
			/// <returns>
			/// Un valor verdadero si logr� posicionarse en el siguiente elemento de
			/// la enumeraci�n, un valor igual a falso cuando no existen mas elementos
			/// a enumerar.
			/// </returns>
			public bool MoveNext() {

				return _fieldsEnumerator.MoveNext();
			}

			/// <summary>
			/// Retorna el elemento actual de la enumeraci�n.
			/// </summary>
			public object Current {

				get {

					return ( ( DictionaryEntry)_fieldsEnumerator.Current).Value;
				}
			}
			#endregion
		}
		#endregion
	}
}
