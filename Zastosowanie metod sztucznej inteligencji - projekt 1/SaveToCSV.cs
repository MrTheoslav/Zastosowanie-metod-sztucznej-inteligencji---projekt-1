using CsvHelper;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zastosowanie_metod_sztucznej_inteligencji___projekt_1
{
    public class SaveToCSV
    {
        public void Save(string filePath, List<TableOfResults> table)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(table);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }
        }
    }
}
