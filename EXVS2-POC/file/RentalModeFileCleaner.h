#pragma once

#include <filesystem>
#include "../Version.h"

void clean_up_rental_mode_file(std::filesystem::path&& basePath, GameVersion game_version);