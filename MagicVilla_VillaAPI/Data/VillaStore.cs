﻿using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data
{
    //use data or database
        public static class VillaStore
        {
            public static List<VillaDTO> villalist = new List<VillaDTO>
            {
                new VillaDTO { Id = 1,Name="Pool View",sqft=100,Occupancy=4},
                new VillaDTO { Id = 2,Name="Beach View",sqft=200,Occupancy=3}
            };
        }
}
