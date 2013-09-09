﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExplorer.Application.Importers.CsvFile;
using DataExplorer.Domain.Sources;
using NUnit.Framework;

namespace DataExplorer.Tests.Application.Importers.CsvFile
{
    [TestFixture]
    public class CsvFileSourceAdapterTests
    {
        private CsvFileSourceAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _adapter = new CsvFileSourceAdapter();
        }

        [Test]
        public void TestAdaptShouldAdaptSourceToDto()
        {
            var source = new CsvFileSource() { FilePath = @"C:\Test.csv" };
            var result = _adapter.Adapt(source);
            Assert.That(result.FilePath, Is.EqualTo(@"C:\Test.csv"));
        }
    }
}
