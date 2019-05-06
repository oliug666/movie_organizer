using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Morganizer
{
    public class Exporter
    {
        static readonly int DIRECTOR = 1;
        static readonly int TITLE = 2;
        static readonly int SIZE = 3;

        //string where = "C:\\Users\\c267376\\OneDrive - Eli Lilly and Company\\_WORK\\"; // "H:\\";
        //string name = "movie_info.xlsx";

        public void Export(string where, string name)
        {
            if (System.IO.Directory.Exists(where))
            {
                // Delete the existing file
                if (File.Exists(where + name))
                    File.Delete(where + name);
                // Create the new file
                var fileInfo = new FileInfo(where + name);
                int idxCell = 1;

                using (ExcelPackage p = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add("MovieList");
                    // Create the header
                    ws.Cells[idxCell, DIRECTOR].Value = "DIRECTOR";
                    ws.Cells[idxCell, TITLE].Value = "MOVIE NAME";
                    ws.Cells[idxCell, SIZE].Value = "FILE SIZE [Mb]";
                    idxCell++;

                    // foreach folder (directors)
                    foreach (string folder in Directory.EnumerateDirectories(where))
                    {
                        // foreach file (movies)
                        foreach (string filename in Directory.EnumerateFiles(folder))
                        {
                            if (Path.GetExtension(filename) == ".avi" || Path.GetExtension(filename) == ".mkv" || Path.GetExtension(filename) == ".mp4")
                            {
                                ws.Cells[idxCell, DIRECTOR].Value = Path.GetFileName(folder);
                                ws.Cells[idxCell, TITLE].Value = Path.GetFileName(filename);
                                ws.Cells[idxCell, SIZE].Value = (new FileInfo(filename).Length) / 1024000;
                                idxCell++;
                            }
                        }
                    }

                    // foreach file (movies, no directors)
                    foreach (string filename in Directory.EnumerateFiles(where))
                    {
                        if (Path.GetFileName(filename) != name)
                        {
                            if (Path.GetExtension(filename) == ".avi" || Path.GetExtension(filename) == ".mkv" || Path.GetExtension(filename) == ".mp4")
                            {
                                ws.Cells[idxCell, DIRECTOR].Value = "not-specified";
                                ws.Cells[idxCell, TITLE].Value = Path.GetFileName(filename);
                                ws.Cells[idxCell, SIZE].Value = (new FileInfo(filename).Length) / 1024000;
                                idxCell++;
                            }
                        }
                    }

                    // Aesthetics
                    ws.Column(1).Width = 40;
                    ws.Column(2).Width = 90;
                    ws.Column(3).Width = 25;

                    // Assign borders
                    string modelRange = "A1:C" + (idxCell - 1).ToString();
                    var modelTable = ws.Cells[modelRange];
                    modelTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    modelTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    // Assign alignment
                    modelTable.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    // Assign title bold
                    modelRange = "A1:C1";
                    modelTable = ws.Cells[modelRange];
                    modelTable.Style.Font.Bold = true;
                    modelTable.Style.Font.Size = 16;

                    // Add filter
                    ws.Cells["A1:C1"].AutoFilter = true;

                    p.Save();
                }
            }
        }
    }
}
