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
using System.Collections;

// TODO: Translate spanish -> english.

namespace Fanap.Messaging {

	/// <summary>
	/// Implementa una colección de campos.
	/// </summary>
	/// <remarks>
	/// El número de campo es empleado como clave dentro de la colección.
	/// </remarks>
    [Serializable]
	public class FieldCollection : IEnumerable {

		private Hashtable _fields;
		private int _maxField;
		private bool _dirty;
		private bool _maxFieldDirty;

		#region Constructors
		/// <summary>
		/// Crea una nueva instancia de la colección de campos.
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
		/// Retorna la cantidad de campos incluidos en la colección.
		/// </summary>
		public int Count {

			get {

				return _fields.Count;
			}
		}

		/// <summary>
		/// Retorna el número del campo cuyo número sea el mayor contenido en
		/// la colección.
		/// </summary>
		/// <exception cref="ApplicationException">
		/// La colección se encuentra vacía.
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
		/// Asigna o devuelve un valor de tipo lógico que indica si la colección
		/// ha sufrido modificaciones.
		/// </summary>
		/// <remarks>
		/// Esta propiedad puede ser empleada por el usuario para seguir
		/// la pista de la colección, cuando se agregan o borran campos a la
		/// colección esta propiedad pasa a valer <see langref="true"/>, es
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
		/// Agrega un campo a la colección.
		/// </summary>
		/// <param name="field">
		/// Es el campo a agregar a la colección.
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
		/// Agrega a la colección un campo cuyo valor es una cadena de caracteres.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo a agregar a colección.
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
		/// Agrega a la colección un campo cuyo valor es un array de bytes.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo a agregar a colección.
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
        /// Agrega a la colección un campo cuyo valor es un mensaje.
        /// </summary>
        /// <param name="fieldNumber">
        /// Es el número del campo a agregar a colección.
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
		/// Elimina el campo cuyo número coincida con el especificado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo que se desea eliminar de la colección.
		/// </param>
		public void Remove( int fieldNumber) {

			_fields.Remove( fieldNumber);

			_dirty = true;

			if ( fieldNumber == _maxField) {
				_maxFieldDirty = true;
			}
		}

		/// <summary>
		/// Elimina los campos cuyos números coincidan con los indicados.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Es el conjunto de números de campos a eliminar de la colección.
		/// </param>
		public void Remove( int[] fieldsNumbers) {

			for ( int i = 0; i < fieldsNumbers.Length; i++) {
				Remove( fieldsNumbers[i]);
			}
		}

		/// <summary>
		/// Elimina todos los campos de la colección.
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
		/// Indica si la colección contiene un campo para el número de campo
		/// indicado.
		/// </summary>
		/// <param name="fieldNumber">
		/// Es el número del campo para el que se desea conocer si existe un
		/// campo en la colección.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si el campo está contenido en la colección,
		/// <see langref="false"/> en caso contrario.
		/// </returns>
		public bool Contains( int fieldNumber) {

			return _fields.Contains( fieldNumber);
		}

		/// <summary>
		/// Indica si la colección contiene todos los campos cuyos números se
		/// indican.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Son los números de campos para los que se desea conocer si existen
		/// en la colección.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si todos los campos están contenidos en la
		/// colección, <see langref="false"/> en caso contrario.
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
		/// Indica si la colección contiene por lo menos uno de los campos
		/// cuyos números se indican.
		/// </summary>
		/// <param name="fieldsNumbers">
		/// Son los números de campos para los que se desea conocer si existe
		/// al menos uno en la colección.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si uno de los campos está contenidos en la
		/// colección, <see langref="false"/> en caso contrario.
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
		/// Indica si la colección contiene por lo menos uno de los campos
		/// cuyos números se encuentran en un rango dado.
		/// </summary>
		/// <param name="lowerFieldNumber">
		/// Es el número inicial del rango.
		/// </param>
		/// <param name="upperFieldNumber">
		/// Es el número final del rango.
		/// </param>
		/// <returns>
		/// <see langref="true"/> si uno de los campos está contenidos en la
		/// colección, <see langref="false"/> en caso contrario.
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
		/// Cambia el número de un campo por otro dado.
		/// </summary>
		/// <param name="oldFieldNumber">
		/// Es el número de campo que se desea cambiar.
		/// </param>
		/// <param name="newFieldNumber">
		/// Es el nuevo número de campo.
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
		/// Devuelve un enumerador de la colección.
		/// </summary>
		/// <returns>
		/// El enumerador sobre la colección.
		/// </returns>
		public IEnumerator GetEnumerator() {

			return new FieldsEnumerator( _fields);
		}

		/// <summary>
		/// Implementa el enumerador de la colección.
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
			/// Reinicia la enumeración.
			/// </summary>
			public void Reset() {
			
				_fieldsEnumerator.Reset();
			}

			/// <summary>
			/// Se mueve al siguiente elemento en la enumeración.
			/// </summary>
			/// <returns>
			/// Un valor verdadero si logró posicionarse en el siguiente elemento de
			/// la enumeración, un valor igual a falso cuando no existen mas elementos
			/// a enumerar.
			/// </returns>
			public bool MoveNext() {

				return _fieldsEnumerator.MoveNext();
			}

			/// <summary>
			/// Retorna el elemento actual de la enumeración.
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
