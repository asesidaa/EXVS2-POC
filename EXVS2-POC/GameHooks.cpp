// ReSharper disable CppClangTidyClangDiagnosticMicrosoftCast
#include "GameHooks.h"

#include "AmAuthEmu.h"
#include "random.h"
#include "game/CardReaderHooker.h"
#include "game/ClientTypePatcher.h"
#include "game/ContentRouterPatcher.h"
#include "game/DevMenuSubOptionsEnabler.h"
#include "game/NetworkAdapterCheckPatcher.h"
#include "game/OpeningScreenSkipper.h"
#include "game/PcCheckPatcher.h"
#include "game/ResolutionPatcher.h"

using Random = effolkronium::random_static;

static constexpr auto BASE_ADDRESS = 0x140000000;

void InitializeHooks(const GameVersion game_version)
{
    auto exe_base_pointer = reinterpret_cast<uintptr_t>(GetModuleHandle(nullptr));
    patch_pc_check(game_version, exe_base_pointer, BASE_ADDRESS);
    patch_content_router(game_version, exe_base_pointer, BASE_ADDRESS);
    patch_client_type(game_version, exe_base_pointer, BASE_ADDRESS);
    patch_network_adapter_check(game_version, exe_base_pointer, BASE_ADDRESS);
    enable_dev_menu_sub_options(game_version, exe_base_pointer, BASE_ADDRESS);
    skip_opening_screen(game_version, exe_base_pointer, BASE_ADDRESS);
    patch_resolution(game_version, exe_base_pointer, BASE_ADDRESS);
    hook_card_reader_post_process(game_version, exe_base_pointer, BASE_ADDRESS);
}
