using nue.protocol.exvs;

namespace Server.Handlers.Game.LoadGameData;

public class EchelonTableCommand : ILoadGameDataCommand
{
    public void Fill(Response.LoadGameData loadGameData)
    {
        for (uint i = 0; i < 8; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 0,
                UpDefaultExp = 0
            });
        }
        
        for (uint i = 8; i < 13; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 250,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 13,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 14; i < 18; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 700,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 18,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 19; i < 23; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 1200,
                UpDefaultExp = 0
            });
        }

        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 23,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 24; i < 28; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 1000,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 28,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 29; i < 33; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 4000,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 33,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 34; i < 38; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 7000,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 38,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 39; i < 43; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 8000,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 43,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        for (uint i = 44; i < 48; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 13000,
                UpDefaultExp = 0
            });
        }
        
        for (uint i = 48; i < 53; i++)
        {
            loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
            {
                EchelonId = i,
                DownDefaultExp = 18000,
                UpDefaultExp = 0
            });
        }
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 53,
            DownDefaultExp = 26000,
            UpDefaultExp = 0
        });
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 54,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
        
        loadGameData.EchelonTables.Add(new Response.LoadGameData.EchelonTable()
        {
            EchelonId = 55,
            DownDefaultExp = 0,
            UpDefaultExp = 0
        });
    }
}