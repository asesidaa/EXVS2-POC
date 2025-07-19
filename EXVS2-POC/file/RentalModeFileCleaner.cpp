#include "RentalModeFileCleaner.h"

#include "../Configs.h"
#include "../log.h"

void clean_up_rental_mode_file(std::filesystem::path&& basePath, GameVersion game_version)
{
    auto gameSettingBasePath = std::move(basePath);

    if(globalConfig.Mode == 2 || globalConfig.Mode == 4)
    {
        return;
    }
    
    if(game_version != Overboost_480)
    {
        info("Rental Mode File doesn't exist in VS2 / XB, Skip...");
        return;
    }
    
    if (!exists(gameSettingBasePath / "27/time_rental"))
    {
        info("Rental Mode File not yet created in Overboost, Skip...");
        return;
    }

    info("Clean Up Rental Mode File for Overboost before start...");
    remove_all(gameSettingBasePath / "27/time_rental");
}