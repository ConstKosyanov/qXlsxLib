﻿using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using qXlsxLib.Book;
using System;
using System.Linq;
using qXlsxLib.Excel;

namespace qXlsxLib.Reader
{
    internal class xlBookReader : xlReader
    {
        #region Variables
        //=================================================
        xlBook result;
        //=================================================
        #endregion

        #region Constructor
        //=================================================
        internal xlBookReader(SpreadsheetDocument document)
            : base(document)
        {
            result = new xlBook();
        }
        //=================================================
        #endregion

        #region Methods
        //=================================================
        public xlBook ReadToBook()
        {
            for (int i = 0; i < doc.WorkbookPart.WorksheetParts.Count(); i++)
            {
                result.AddSheet(sheetNames[i]);
                var sheet = doc.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>().SingleOrDefault(s => s.Name == sheetNames[i]);
                var worksheetPart = (WorksheetPart)doc.WorkbookPart.GetPartById(sheet.Id.Value);
                foreach (var cell in worksheetPart.Worksheet.GetFirstChild<SheetData>().Descendants<Cell>())
                {
                    if (cell == null) { continue; }
                    var res = ReadCell(cell);
                    result[i].Cells.Add(res);
                }
            }

            return result;
        }
        //=================================================
        #endregion
    }
}