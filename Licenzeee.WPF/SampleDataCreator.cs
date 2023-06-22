using System;
using Fateblade.Licenzee.Db;
using Fateblade.Licenzee.Db.Models;

namespace Fateblade.Licenzeee.WPF;

public class SampleDataCreator
{
    private readonly IDb _db;

    public SampleDataCreator(IDb dbToImportSampleDataInto)
    {
        _db = dbToImportSampleDataInto;
    }

    public void ImportSampleData()
    {
        var product1 = _db.CreateProduct("Magic Wizard 5000", "1.0.2", "Wizarding Community Inc.", string.Empty);
        var product2 = _db.CreateProduct("BootstrapCrusher", "0.5.1", "Crashing Software", "Works as described, crashes all the time");
        var product3 = _db.CreateProduct("Brunkeldunk", "5.1.8", "Blubberdub Wunderwurks.", "Don't ask where we got this");
        var product4 = _db.CreateProduct("Rumpelstilzchen Name Generator", Random.Shared.Next(0,995).ToString("0.00"), "J&W Grimm Software", "Kinda random");


        var user1 = _db.CreateUser("Bob", "Robs best friend");
        var user2 = _db.CreateUser("Bob's Laptop", "In Room A38");
        var user3 = _db.CreateUser("Bob's Desktop", "Runs some very odd OS");
        var user4 = _db.CreateUser("Rob", "Bob hates him");
        var user5 = _db.CreateUser("Rob's Fruity Cylinder", "Very round");
        var user6 = _db.CreateUser("Server-A", "Does anyone kow where we placed that thing?");
        var user7 = _db.CreateUser("GZK 8999", "Note: Don't turn it on again, please");


        _db.CreateLicense("Licenzeee Now You Zeee", product1.Id, UsageType.Comment, "Expires when the mana subsides to the 5th degree of Oz, the contract is quite arcane");
        _db.CreateLicense("BRU.NKE.LDU.NK", product3.Id, UsageType.Comment, "No wonder they dont sell well");
        _db.CreateLicense("21BM-33FF-BMKS-NDS1-BBK0", product2.Id, UsageType.SingleUser, string.Empty, user1);
        _db.CreateLicense("21BM-33FF-BMKS-NDS2-BBK0", product2.Id, UsageType.SingleUser, string.Empty, user4);
        _db.CreateLicense(Guid.NewGuid().ToString().ToUpper(), product4.Id, UsageType.MultiUser, string.Empty, user2, user3, user5, user6);
    }
}