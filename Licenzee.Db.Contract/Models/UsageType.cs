﻿namespace Fateblade.Licenzee.Db.Models;

public struct UsageType
 {
     public int Id { get; }
     public string Name { get; }

     public UsageType(int id, string name)
     {
         Id = id;
         Name = name;
     }
}