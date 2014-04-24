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
using Fanap.Utilities;

namespace Fanap.Messaging.Iso8583
{

    /// <summary>
    /// It defines an ISO 8583 messages formatter to produce and parse
    /// strings without control characters. The definition is for the
    /// ISO 8583-87.
    /// </summary>
    public class Iso8583Ascii1987MessageFormatter : Iso8583MessageFormatter
    {

        #region Constructors
        /// <summary>
        /// It initializes a new ISO 8583 formatter.
        /// </summary>
        public Iso8583Ascii1987MessageFormatter()
            : base()
        {

            SetupFields();
        }
        #endregion

        #region Methods
        /// <summary>
        /// It initializes the fields formatters for this message formatter.
        /// </summary>
        private void SetupFields()
        {

            MessageTypeIdentifierFormatter = new StringFieldFormatter(
                -1, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Message type identifier");

            FieldFormatters.Add(new BitMapFieldFormatter(0, 1, 64,
                HexadecimalBinaryEncoder.GetInstance(), "Primary bitmap"));
            FieldFormatters.Add(new BitMapFieldFormatter(1, 65, 128,
                HexadecimalBinaryEncoder.GetInstance(), "Secondary bitmap"));
            FieldFormatters.Add(new StringFieldFormatter(
                2, new VariableLengthManager(0, 19, StringLengthEncoder.GetInstance(19)),
                StringEncoder.GetInstance(), NumericValidator.GetInstance(),
                "Primary account number"));
            FieldFormatters.Add(new StringFieldFormatter(
                3, new FixedLengthManager(6), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Processing code"));
            FieldFormatters.Add(new StringFieldFormatter(
                4, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Transaction amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                5, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Reconciliation amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                6, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Cardholder billing amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                7, new FixedLengthManager(14), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Transmission date and time"));
            FieldFormatters.Add(new StringFieldFormatter(
                8, new FixedLengthManager(8), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Cardholder billing fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                9, new FixedLengthManager(8), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Reconciliation conversion rate"));
            FieldFormatters.Add(new StringFieldFormatter(
                10, new FixedLengthManager(8), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Cardholder billing conversion rate"));
            FieldFormatters.Add(new StringFieldFormatter(
                11, new FixedLengthManager(6), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Systems trace audit number"));
            FieldFormatters.Add(new StringFieldFormatter(
                12, new FixedLengthManager(6), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Local transaction time"));
            FieldFormatters.Add(new StringFieldFormatter(
                13, new FixedLengthManager(8), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Local transaction date"));
            FieldFormatters.Add(new StringFieldFormatter(
                14, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Expiration date"));
            FieldFormatters.Add(new StringFieldFormatter(
                15, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Settlement date"));
            FieldFormatters.Add(new StringFieldFormatter(
                16, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Conversion date"));
            FieldFormatters.Add(new StringFieldFormatter(
                17, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Capture date"));
            FieldFormatters.Add(new StringFieldFormatter(
                18, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Merchant type"));
            FieldFormatters.Add(new StringFieldFormatter(
                19, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Acquiring institution country code"));
            FieldFormatters.Add(new StringFieldFormatter(
                20, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Primary account number country code"));
            FieldFormatters.Add(new StringFieldFormatter(
                21, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Forwarding institution country code"));
            FieldFormatters.Add(new StringFieldFormatter(
                22, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Point of service record mode"));
            FieldFormatters.Add(new StringFieldFormatter(
                23, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Card sequence number"));
            FieldFormatters.Add(new StringFieldFormatter(
                24, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Network international identifier"));
            FieldFormatters.Add(new StringFieldFormatter(
                25, new FixedLengthManager(2), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Point of service condition code"));
            FieldFormatters.Add(new StringFieldFormatter(
                26, new FixedLengthManager(2), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Point of service PIN capture code"));
            FieldFormatters.Add(new StringFieldFormatter(
                27, new FixedLengthManager(1), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Authorization Identification response length"));
            FieldFormatters.Add(new StringFieldFormatter(
                28, new FixedLengthManager(9), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Transaction fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                29, new FixedLengthManager(9), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Settlement fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                30, new FixedLengthManager(9), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Transaction processing fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                31, new FixedLengthManager(9), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Settlement processing fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                32, new VariableLengthManager(0, 11, StringLengthEncoder.GetInstance(11)),
                StringEncoder.GetInstance(), NumericValidator.GetInstance(),
                "Acquirer institution identification code"));
            FieldFormatters.Add(new StringFieldFormatter(
                33, new VariableLengthManager(0, 11, StringLengthEncoder.GetInstance(11)),
                StringEncoder.GetInstance(), NumericValidator.GetInstance(),
                "Forwarding institution identification code"));
            FieldFormatters.Add(new StringFieldFormatter(
                34, new VariableLengthManager(0, 28, StringLengthEncoder.GetInstance(28)),
                StringEncoder.GetInstance(), "Extended primary account number"));
            FieldFormatters.Add(new StringFieldFormatter(
                35, new VariableLengthManager(0, 37, StringLengthEncoder.GetInstance(37)),
                StringEncoder.GetInstance(), "Track 2 data"));
            FieldFormatters.Add(new StringFieldFormatter(
                36, new VariableLengthManager(0, 104, StringLengthEncoder.GetInstance(104)),
                StringEncoder.GetInstance(), "Track 3 data"));
            FieldFormatters.Add(new StringFieldFormatter(
                37, new FixedLengthManager(12), StringEncoder.GetInstance(),
                "Retrieval reference number"));
            FieldFormatters.Add(new StringFieldFormatter(
                38, new FixedLengthManager(6), StringEncoder.GetInstance(),
                "Authorization identification response"));
            FieldFormatters.Add(new StringFieldFormatter(
                39, new FixedLengthManager(2), StringEncoder.GetInstance(),
                "Response code"));
            FieldFormatters.Add(new StringFieldFormatter(
                40, new FixedLengthManager(3), StringEncoder.GetInstance(),
                "Service restriction code"));
            FieldFormatters.Add(new StringFieldFormatter(
                41, new FixedLengthManager(8), StringEncoder.GetInstance(),
                "Card acceptor terminal identification"));
            FieldFormatters.Add(new StringFieldFormatter(
                42, new FixedLengthManager(15), StringEncoder.GetInstance(),
                "Card acceptor identification code"));
            FieldFormatters.Add(new StringFieldFormatter(
                43, new FixedLengthManager(40), StringEncoder.GetInstance(),
                "Card acceptor name/location"));
            FieldFormatters.Add(new StringFieldFormatter(
                44, new VariableLengthManager(0, 99, StringLengthEncoder.GetInstance(25)),
                StringEncoder.GetInstance(), "Additional response data"));
            FieldFormatters.Add(new StringFieldFormatter(
                45, new VariableLengthManager(0, 76, StringLengthEncoder.GetInstance(76)),
                StringEncoder.GetInstance(), "Track 1 data"));
            FieldFormatters.Add(new StringFieldFormatter(
                46, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Additional data - ISO"));
            FieldFormatters.Add(new StringFieldFormatter(
                47, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Additional data - national"));
            FieldFormatters.Add(new StringFieldFormatter(
                48, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Additional data - private"));
            FieldFormatters.Add(new StringFieldFormatter(
                49, new FixedLengthManager(3), StringEncoder.GetInstance(),
                "Transaction currency code"));
            FieldFormatters.Add(new StringFieldFormatter(
                50, new FixedLengthManager(3), StringEncoder.GetInstance(),
                "Reconciliation currency code"));
            FieldFormatters.Add(new StringFieldFormatter(
                51, new FixedLengthManager(3), StringEncoder.GetInstance(),
                "Cardholder billing currency code"));
            FieldFormatters.Add(new StringFieldFormatter(
               52, new FixedLengthManager(16), StringEncoder.GetInstance(),
               ZeroPaddingLeft.GetInstance(false, false), "Personal identification number (PIN) data"));
            FieldFormatters.Add(new StringFieldFormatter(
                53, new VariableLengthManager(0, 48, StringLengthEncoder.GetInstance(48)),
                StringEncoder.GetInstance(), "Security related control information"));
            //FieldFormatters.Add( new StringFieldFormatter(
            //    53, new FixedLengthManager( 16 ), StringEncoder.GetInstance(),
            //    ZeroPaddingLeft.GetInstance( false, false ), "Security related control information" ) );
            FieldFormatters.Add(new StringFieldFormatter(
                54, new VariableLengthManager(0, 120, StringLengthEncoder.GetInstance(120)),
                StringEncoder.GetInstance(), "Amounts, additional"));
            FieldFormatters.Add(new StringFieldFormatter(
                55, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                56, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                57, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                58, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                59, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                60, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                61, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                62, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                63, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
               64, new FixedLengthManager(16), StringEncoder.GetInstance(),
               ZeroPaddingLeft.GetInstance(false, false), "Message authentication code field"));
            FieldFormatters.Add(new BinaryFieldFormatter(
                65, new FixedLengthManager(8), BinaryEncoder.GetInstance(),
                "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                66, new FixedLengthManager(1), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Settlement code"));
            FieldFormatters.Add(new StringFieldFormatter(
                67, new FixedLengthManager(2), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Extended payment data"));
            FieldFormatters.Add(new StringFieldFormatter(
                68, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Receiving institution country code"));
            FieldFormatters.Add(new StringFieldFormatter(
                69, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Settlement institution country code"));
            FieldFormatters.Add(new StringFieldFormatter(
                70, new FixedLengthManager(3), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Network management information code"));
            FieldFormatters.Add(new StringFieldFormatter(
                71, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Message number"));
            FieldFormatters.Add(new StringFieldFormatter(
                72, new FixedLengthManager(4), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Last message number"));
            FieldFormatters.Add(new StringFieldFormatter(
                73, new FixedLengthManager(6), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Action date"));
            FieldFormatters.Add(new StringFieldFormatter(
                74, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, number"));
            FieldFormatters.Add(new StringFieldFormatter(
                75, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, reversal number"));
            FieldFormatters.Add(new StringFieldFormatter(
                76, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, number"));
            FieldFormatters.Add(new StringFieldFormatter(
                77, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, reversal number"));
            FieldFormatters.Add(new StringFieldFormatter(
                78, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Transfer, number"));
            FieldFormatters.Add(new StringFieldFormatter(
                79, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Transfer, reversal number"));
            FieldFormatters.Add(new StringFieldFormatter(
                80, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Inquiries, number"));
            FieldFormatters.Add(new StringFieldFormatter(
                81, new FixedLengthManager(10), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Authorizations, number"));
            FieldFormatters.Add(new StringFieldFormatter(
                82, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, processing fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                83, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, transaction fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                84, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, processing fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                85, new FixedLengthManager(12), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, transaction fee amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                86, new FixedLengthManager(16), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                87, new FixedLengthManager(16), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Credits, reversal amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                88, new FixedLengthManager(16), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                89, new FixedLengthManager(16), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, true), "Debits, reversal amount"));
            FieldFormatters.Add(new StringFieldFormatter(
                90, new FixedLengthManager(42), StringEncoder.GetInstance(),
                ZeroPaddingLeft.GetInstance(false, false), "Original Data Elements"));
            FieldFormatters.Add(new StringFieldFormatter(
                91, new FixedLengthManager(1), StringEncoder.GetInstance(),
                "File update code"));
            FieldFormatters.Add(new StringFieldFormatter(
                92, new FixedLengthManager(2), StringEncoder.GetInstance(),
                "File security code"));
            FieldFormatters.Add(new StringFieldFormatter(
                93, new FixedLengthManager(6), StringEncoder.GetInstance(),
                "Response indicator"));
            FieldFormatters.Add(new StringFieldFormatter(
                94, new FixedLengthManager(7), StringEncoder.GetInstance(),
                "Service indicator"));
            FieldFormatters.Add(new StringFieldFormatter(
                95, new FixedLengthManager(42), StringEncoder.GetInstance(),
                "Replacement amounts"));
            FieldFormatters.Add(new BinaryFieldFormatter(
                96, new FixedLengthManager(16), HexadecimalBinaryEncoder.GetInstance(),
                "Message security code"));
            FieldFormatters.Add(new StringFieldFormatter(
                97, new FixedLengthManager(17), StringEncoder.GetInstance(),
                "Amount, net settlement"));
            FieldFormatters.Add(new StringFieldFormatter(
                98, new FixedLengthManager(25), StringEncoder.GetInstance(),
                "Payee"));
            FieldFormatters.Add(new StringFieldFormatter(
                99, new VariableLengthManager(0, 11, StringLengthEncoder.GetInstance(11)),
                StringEncoder.GetInstance(), NumericValidator.GetInstance(),
                "Settlement institution Id code"));
            FieldFormatters.Add(new StringFieldFormatter(
                100, new VariableLengthManager(0, 11, StringLengthEncoder.GetInstance(11)),
                StringEncoder.GetInstance(), NumericValidator.GetInstance(),
                "Receiving institution Id code"));
            FieldFormatters.Add(new StringFieldFormatter(
                101, new VariableLengthManager(0, 17, StringLengthEncoder.GetInstance(17)),
                StringEncoder.GetInstance(), "File name"));
            FieldFormatters.Add(new StringFieldFormatter(
                102, new VariableLengthManager(0, 28, StringLengthEncoder.GetInstance(28)),
                StringEncoder.GetInstance(), "Account identification 1"));
            FieldFormatters.Add(new StringFieldFormatter(
                103, new VariableLengthManager(0, 28, StringLengthEncoder.GetInstance(28)),
                StringEncoder.GetInstance(), "Account identification 2"));
            FieldFormatters.Add(new StringFieldFormatter(
                104, new VariableLengthManager(0, 100, StringLengthEncoder.GetInstance(100)),
                StringEncoder.GetInstance(), "Transaction description"));
            FieldFormatters.Add(new StringFieldFormatter(
                105, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                106, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                107, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                108, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                109, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                110, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                111, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for ISO use"));
            FieldFormatters.Add(new StringFieldFormatter(
                112, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                113, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                114, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                115, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                116, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                117, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                118, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                119, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for national use"));
            FieldFormatters.Add(new StringFieldFormatter(
                120, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                121, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                122, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                123, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                124, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                125, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                126, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
                127, new VariableLengthManager(0, 999, StringLengthEncoder.GetInstance(999)),
                StringEncoder.GetInstance(), "Reserved for private use"));
            FieldFormatters.Add(new StringFieldFormatter(
               128, new FixedLengthManager(16), StringEncoder.GetInstance(),
               ZeroPaddingLeft.GetInstance(false, false), "Message authentication code field"));
        }

        /// <summary>
        /// It clones the formatter instance.
        /// </summary>
        /// <remarks>
        /// The header, the mti formatter and the fields formatters, aren't cloned,
        /// the new instance and the original shares those object instances.
        /// </remarks>
        /// <returns>
        /// A new instance of the formatter.
        /// </returns>
        public override object Clone()
        {

            Iso8583Ascii1987MessageFormatter formatter = new Iso8583Ascii1987MessageFormatter();

            CopyTo(formatter);

            return formatter;
        }
        #endregion
    }
}
