﻿#pragma once
#include <vector>

static std::vector<uint64_t> PATCH_TARGETS_OVERBOOST_V25 = {
    0x1BE815, 0x1BE963, 0x1BE9B4, 0x1BEC08, 0x1BED16, 0x1BEEDE, 0x1BEFC9, 0x2F6C0D, 0x2F7F2A, 0x2FA5B9, 0x32E64E,
    0x32EA58, 0x3319FD, 0x33C446, 0x33C692, 0x35143A, 0x353131, 0xAA1EFF, 0xAA21C4, 0xAA226B, 0xAA2436, 0xAA2FC1,
    0xAA35B0, 0xAA4107, 0xAA4F79, 0xAA5078, 0xAA542E, 0xAA7CBB, 0xAA9FB7, 0xAAA4D5, 0xAAC850, 0xAACAE8, 0xAACDA1,
    0xAAF05A, 0xAAF0A2, 0xAB1803, 0xAB1944, 0xAB1981, 0xAB2299, 0xAB2312, 0xAB8379, 0xABD023, 0xABDC0F, 0xABDC68,
    0xABECF0, 0xABEDA1, 0xABEDF2, 0xABF46A, 0xABF516, 0xABF567, 0xABF917, 0xABF964, 0xABF999, 0xAC5BE9, 0xAC74FC,
    0xAC8E69, 0xAC9DF0, 0xACBD8B, 0xACDB7D, 0xACDCE5, 0xACDD52, 0xACDD91, 0xACDF30, 0xACDF4B, 0xACE0F6, 0xACE237,
    0xACE280, 0xACE400, 0xACE42F, 0xACE6D7, 0xACF3BB, 0xACF40E, 0xACF477, 0xACF558, 0xACF73F, 0xACF80D, 0xACF8C6,
    0xACF96F, 0xACF9D8, 0xACF9FD, 0xACFB9D, 0xACFBE6, 0xACFCAA, 0xACFD4A, 0xACFD6B, 0xACFE4E, 0xACFEB1, 0xACFF29,
    0xAD003C, 0xAD00B8, 0xAD014A, 0xAD019B, 0xAD042C, 0xAD0484, 0xAD04FD, 0xAD05D4, 0xAD07CC, 0xAD089C, 0xAD0AE7,
    0xAD198E, 0xAD1B03, 0xAD1B70, 0xAD1BAF, 0xAD1D4E, 0xAD1D69, 0xAD1F14, 0xAD2051, 0xAD209A, 0xAD221A, 0xAD2249,
    0xAD24FB, 0xAD31FA, 0xAD324D, 0xAD32B7, 0xAD3399, 0xAD357F, 0xAD3651, 0xAD3713, 0xAD37AC, 0xAD381B, 0xAD3840,
    0xAD39E0, 0xAD3A39, 0xAD3AE9, 0xAD3B9A, 0xAD3BBB, 0xAD3C9E, 0xAD3D01, 0xAD3D79, 0xAD3E95, 0xAD3F11, 0xAD3FA3,
    0xAD3FF4, 0xAD4239, 0xAD4291, 0xAD4302, 0xAD43DF, 0xAD45DA, 0xAD46AA, 0xAD48FD, 0xAD51DE, 0xADB073, 0xADE547,
    0xADE6C1, 0xADE72D, 0xADE76D, 0xADE879, 0xADE92E, 0xADE94E, 0xADEB16, 0xADEC73, 0xADECC0, 0xADEE4A, 0xADEE69,
    0xADF122, 0xADFE27, 0xADFE7A, 0xADFEE4, 0xADFFC8, 0xAE01C7, 0xAE0297, 0xAE0361, 0xAE03F5, 0xAE0466, 0xAE048B,
    0xAE0634, 0xAE0676, 0xAE0741, 0xAE07E6, 0xAE0803, 0xAE08E5, 0xAE0953, 0xAE09CB, 0xAE0ADD, 0xAE0B4E, 0xAE0BF1,
    0xAE0C3B, 0xAE0E6E, 0xAE0EC6, 0xAE0F3F, 0xAE1011, 0xAE1207, 0xAE12C8, 0xAE1534, 0xAE3625, 0xAE69AD, 0xAEA5E8,
    0xAECB72, 0xAECC5C, 0xAECC8F, 0xAEF20A, 0xAEF2B1, 0xAEF6CD, 0xAEF7F6, 0xAEFBBF, 0xAEFC80, 0xAF0935, 0xAF0A5C,
    0xAF5637, 0xAF573C, 0xAF59EB, 0xAF5C88, 0xAF5EAF, 0xB02DE6, 0xB031E2, 0xB06AEB, 0xB07635, 0xB07864, 0xB0AD33,
    0xB0B3D0, 0xB0B619, 0xB0B8F7, 0xB0BBCE, 0xB0BD01, 0xB0BF9D, 0xB0DF89, 0xB0DFD3, 0xB0E00A, 0xB0E03A, 0xB0E04D,
    0xB0E183, 0xB0E308, 0xB0E641, 0xB0E723, 0xB0E794, 0xB0EAE8, 0xB1130A, 0xB11382, 0xB122D1, 0xB1233A, 0xB12AC3,
    0xB12B63, 0xB12CB5, 0xB12FA1, 0xB12FDB, 0xB13368, 0xB1357B, 0xB135CC, 0xB13779, 0xB14B36, 0xB150A8, 0xB1510C,
    0xB15185, 0xB176E4, 0xB1787A, 0xB17918, 0xB17BBD, 0xB17DFA, 0xB1805E, 0xB18244, 0xB18455, 0xB1856F, 0xB1865B,
    0xB18905, 0xB189A3, 0xB19184, 0xB1F5EF, 0xB20050, 0xB2095B, 0xB21355, 0xB245BA, 0xB246E3, 0xB24E91, 0xB24FB7,
    0xB257DC, 0xB25917, 0xB260C1, 0xB261FC, 0xB276E3, 0xB27F0A, 0xB28631, 0xB28DE7, 0xB29DA1, 0xB29F2A, 0xB2A924,
    0xB2AA5C, 0xB2B509, 0xB2B641, 0xB2BBA0, 0xB2BCBC, 0xB2BF84, 0xB2C0CD, 0xB2C457, 0xB2C6EE, 0xB2D50A, 0xB2DA40,
    0xB2DF3A, 0xB2E68A, 0xB308A0, 0xB30A2F, 0xB30BAE, 0xB31512, 0xB32E1D, 0xB33298, 0xB35E67, 0xB36542, 0xB36AE2,
    0xB38294, 0xB388A6, 0xB38E42, 0xB3BF74, 0xB3C5A4, 0xB3CB42, 0xB3FC71, 0xB3FDF8, 0xB3FF9E, 0xB40D37, 0xB40F68,
    0xB41237, 0xB41368, 0xB41501, 0xB415D6, 0xB4191E, 0xB41A88, 0xB41B50, 0xB41BFD, 0xB41C91, 0xB41CF6, 0xB41D29,
    0xB41EAD, 0xB41EE6, 0xB41FA1, 0xB4202F, 0xB42054, 0xB4212E, 0xB4218A, 0xB421FA, 0xB422FB, 0xB4237F, 0xB42407,
    0xB4245D, 0xB425ED, 0xB42626, 0xB426A6, 0xB4276E, 0xB42919, 0xB42955, 0xB429D5, 0xB42AA7, 0xB42CE6, 0xB42E5A,
    0xB42FAC, 0xB4353A, 0xB43598, 0xB4376C, 0xB43907, 0xB43AA6, 0xB441EF, 0xB44227, 0xB442AE, 0xB4443C, 0xB44474,
    0xB44642, 0xB44660, 0xB44698, 0xB4476A, 0xB447F7, 0xB4483B, 0xB4565C, 0xB4596F, 0xB45ADE, 0xB465BF, 0xB467B3,
    0xB46F5B, 0xB47027, 0xB470B9, 0xB470DB, 0xB4758C, 0xB47815, 0xB4784E, 0xB478CF, 0xB47981, 0xB47AF3, 0xB47BA4,
    0xB47C33, 0xB47CBF, 0xB47D14, 0xB47D40, 0xB47E93, 0xB47EA7, 0xB47F6B, 0xB47FE6, 0xB4800C, 0xB480BF, 0xB48106,
    0xB4816C, 0xB48234, 0xB4829C, 0xB4831A, 0xB48366, 0xB486D7, 0xB48746, 0xB48C29, 0xB48CB2, 0xB4910F, 0xB4938D,
    0xB493C1, 0xB49436, 0xB494F0, 0xB49904, 0xB4A846, 0xB4BB4A, 0xB5AE08, 0xB5AE1E, 0xB62187, 0xB623F8, 0xB62500,
    0xB6259F, 0xB66916, 0xB70A18, 0xB737B6, 0xB73803, 0xB73E7C, 0xB73FA5, 0xB742AE, 0xB742F1, 0xB74375, 0xB746C6,
    0xB747EF, 0xB74AFC, 0xB74B3F, 0xB74BC3, 0xB75037, 0xB75083, 0xB75104, 0xB83586, 0xB83682, 0xB84E78, 0xB84F91,
    0xB85046, 0xB850F8, 0xB86CC2, 0xB86D68, 0xB87533, 0xB875DA, 0xB880CE, 0xB88470, 0xB884DB, 0xB8854E, 0xB8F856,
    0xB8F966, 0xB93C95, 0xB93CA2, 0xB93CC3, 0xB93E37, 0xB93F4D, 0xB93F63, 0xB94040, 0xB941B0, 0xB942FA, 0xB943DF,
    0xB9C3C9, 0xB9C748, 0xBA3DE2, 0xBA3E21, 0xBA3FCE, 0xBA4009, 0xBA41C7, 0xBA4201, 0xBA483F, 0xBA4864, 0xBA4ADD,
    0xBA4B64, 0xBA5344, 0xBA56D0, 0xBA5767, 0xBA59A2, 0xBA5EF9, 0xBA5F63, 0xBA5F9E, 0xBA6011, 0xBA60A8, 0xBA6323,
    0xBA632D, 0xBA65F5, 0xBA66B3, 0xBA670F, 0xBA70AC, 0xBA71C7, 0xBA724D, 0xBA7841, 0xBA7A2D, 0xBA95AE, 0xBA9629,
    0xBA9855, 0xBA98A4, 0xBA9AF8, 0xBA9C54, 0xBA9CA6, 0xBAA01F, 0xBAA054, 0xBAA4DB, 0xBAA874, 0xBAA9BE, 0xBAAA30,
    0xBAAA9C, 0xBAC62C, 0xBACB6C, 0xBAD0A6, 0xBAD815, 0xBAD82D, 0xBADC24, 0xBADF09, 0xBADF67, 0xBADFF5, 0xBAE34E,
    0xBAE366, 0xBAE47C, 0xBAE4E4, 0xBAE5AB, 0xBAED4F, 0xBAED67, 0xBAF1FA, 0xBAF4F9, 0xBAF55F, 0xBAF5E8, 0xBAFABD,
    0xBAFACA, 0xBAFC09, 0xBAFC80, 0xBAFD7D, 0xBB0836, 0xBB084B, 0xBB0C4F, 0xBB0F2E, 0xBB0F7B, 0xBB102E, 0xBB136C,
    0xBB1384, 0xBB1497, 0xBB14FF, 0xBB15C3, 0xBB1DCD, 0xBB1DE8, 0xBB2289, 0xBB257E, 0xBB25D8, 0xBB268E, 0xBB2B39,
    0xBB2B51, 0xBB2C8D, 0xBB2CEB, 0xBB2DFB, 0xBB3B9B, 0xBB49A3, 0xBB49FA, 0xBB4AAA, 0xBB4DF4, 0xBB4E0D, 0xBB4F12,
    0xBB4F78, 0xBB5037, 0xBB5102, 0xBB515C, 0xBB520D, 0xBB56CA, 0xBB56D7, 0xBB5819, 0xBB5887, 0xBB59AA, 0xBB5AC9,
    0xBB6A13, 0xBB6A45, 0xBB6A94, 0xBB7097, 0xBB70CD, 0xBB711C, 0xBB7DC8, 0xBB81D1, 0xBB8204, 0xBB8255, 0xBB8A20,
    0xBB8A58, 0xBB8AAA, 0xBB91E4, 0xBB921C, 0xBB9267, 0xBBBC97, 0xBBBD64, 0xBBBD91, 0xBBBF61, 0xBBBF7C, 0xBBC174,
    0xBBC268, 0xBBC2AD, 0xBBC2B9, 0xBBC474, 0xBBC493, 0xBC1DF7, 0xBC31F8, 0xBC3537, 0xBC3F13, 0xBC41EB, 0xBC4534,
    0xBC4814, 0xBC5AF9, 0xBDA1EB, 0xBDA380, 0xBDB15E, 0xBDB27C, 0xBDB720, 0xBDBF52, 0xBE0E41, 0xBE1570, 0xBE1A4B,
    0xBE1C69, 0xBE1D14, 0xBE43FD, 0xBE444D, 0xBE44E8, 0xBE4538, 0xBE5145, 0xBE5220, 0xBE5357, 0xBE538E, 0xBE54EC,
    0xBE6A56, 0xBE6AF4, 0xBEB660, 0xBEB854, 0xBEBA4D, 0xBEC528, 0xBEC780, 0xBECB27, 0xBECD09, 0xBECD3F, 0xBEDBAC,
    0xBEDC74, 0xBEDE17, 0xBEDEAF, 0xBEDF6C, 0xBEDF8C, 0xBEE159, 0xBEF358, 0xBF0C6A, 0xBF19B9, 0xBF1A69, 0xBF32C4,
    0xBF49ED, 0xBF4F86, 0xBF500B, 0xBF5043, 0xBF7389, 0xBFD867, 0xBFD8C8, 0xBFD935, 0xBFE03E, 0xBFE097, 0xBFE14E,
    0xBFE34A, 0xBFE365, 0xBFE59E, 0xBFF9A6, 0xBFFA75, 0xBFFB9E, 0xC0067C, 0xC00A2F, 0xC00C42, 0xC01299, 0xC0141A,
    0xC01961, 0xC01D1D, 0xC04619, 0xC04984, 0xC04F17, 0xC050A5, 0xC050D4, 0xC050FC, 0xC05163, 0xC051A1, 0xC051D8,
    0xC05EED, 0xC07896, 0xC07BE3, 0xC07CB7, 0xC08575, 0xC085E6, 0xC086A9, 0xC0874A, 0xC08A3B, 0xC0A11C, 0xC0C30A,
    0xC0C96F, 0xC0C9C1, 0xC0DBBA, 0xC0DE75, 0xC0EBB5, 0xC0FA2C, 0xC0FB0D, 0xC0FD36, 0xC0FDFC, 0xC100A9, 0xC1071E,
    0xC1078F, 0xC10862, 0xC10907, 0xC10C8C, 0xC11855, 0xC11970, 0xC11A0D, 0xC13BB5, 0xC13F44, 0xC13F68, 0xC13F7B,
    0xC1411B, 0xC1412B, 0xC14145, 0xC142D6, 0xC1474F, 0xC14782, 0xC14917, 0xC1493B, 0xC14951, 0xC14C26, 0xC14C8B,
    0xC14E9F, 0xC155EA, 0xC17343, 0xC17DAA, 0xC17EB2, 0xC1811C, 0xC184A3, 0xC184BB, 0xC184CE, 0xC18663, 0xC1867B,
    0xC18695, 0xC1881B, 0xC18C96, 0xC18CC9, 0xC18E5F, 0xC18E84, 0xC18E9A, 0xC19157, 0xC191D1, 0xC193E0, 0xC19AD3,
    0xC19D4D, 0xC1AECC, 0xC1AF9E, 0xC1B0FD, 0xC1B273, 0xC1B72D, 0xC1B90A, 0xC20FC5, 0xC21020, 0xC2109D, 0xC210FE,
    0xC21E2D, 0xC22198, 0xC2248D, 0xC22642, 0xC22727, 0xC2275A, 0xC22A16, 0xC22AAC, 0xC22B3B, 0xC22BA5, 0xC22C4A,
    0xC22CA2, 0xC232FC, 0xC23381, 0xC233D7, 0xC234A7, 0xC23503, 0xC23E67, 0xC23EC2, 0xC23F38, 0xC23F99, 0xC24969,
    0xC249C0, 0xC24A65, 0xC24AC1, 0xC251E7, 0xC26B45, 0xC26BA0, 0xC26C23, 0xC26C84, 0xC2845C, 0xC2850D, 0xC28588,
    0xC2863F, 0xC28A55, 0xC290E3, 0xC3622B, 0xC48700, 0xC48827, 0xC48901, 0xC49190, 0xC49F58, 0xC574AE, 0xC577E1,
    0xC57C6A, 0xC57DED, 0xC5886D, 0xC58B4F, 0xC58FC7, 0xC59245, 0xC5935C, 0xC593B9, 0xC5941B, 0xC59587, 0xC5967C,
    0xC59769, 0xC59ADC, 0xC5A349, 0xC5A43A, 0xC5AD01, 0xC5B1BD, 0xC5B2C3, 0xC5B641, 0xC5B7F2, 0xC5B9CA, 0xC5C788,
    0xC5CCDC, 0xC5CDA1, 0xC5CF84, 0xC5CFE5, 0xC5D407, 0xC5D4E0, 0xC5D6A1, 0xC5D8B6, 0xC5DAB9, 0xC5DBBA, 0xC5DCAA,
    0xC5DD25, 0xC5E42D, 0xC5E47E, 0xC5E514, 0xC5E599, 0xC5E5EA, 0xC5F1F7, 0xC5F249, 0xC5FE16, 0xC5FE52, 0xC5FEE8,
    0xC5FFD0, 0xC600E8, 0xC603A9, 0xC6053A, 0xC60615, 0xC606CD, 0xC60764, 0xC607C6, 0xC60818, 0xC609A8, 0xC609EA,
    0xC60AA8, 0xC60B40, 0xC60B62, 0xC60C52, 0xC60CAA, 0xC60D12, 0xC60E1E, 0xC60E8C, 0xC60F25, 0xC60F80, 0xC6113F,
    0xC61174, 0xC61202, 0xC6129A, 0xC612EF, 0xC614B4, 0xC614E6, 0xC6157A, 0xC61665, 0xC618D7, 0xC61A6C, 0xC61BD3,
    0xC64F7E, 0xC64FDB, 0xC65235, 0xC65241, 0xC657AA, 0xC657D5, 0xC6585D, 0xC6589A, 0xC65C55, 0xC65CC1, 0xC65D8C,
    0xC65E20, 0xC65F92, 0xC6602B, 0xC66392, 0xC6643D, 0xC664BE, 0xC664DC, 0xC66502, 0xC66904, 0xC673C7, 0xC675DF,
    0xC67687, 0xC677C3, 0xC6781A, 0xC67A09, 0xC67BC2, 0xC67FF0, 0xC68237, 0xC6828E, 0xC683A8, 0xC683EE, 0xC68629,
    0xC6897A, 0xC68A04, 0xC68BA2, 0xC68C17, 0xC68C71, 0xC68DAA, 0xC69082, 0xC6921B, 0xC693B8, 0xC693FE, 0xC6970F,
    0xC69847, 0xC7B65D, 0xC9295E, 0xC92B3C, 0xC93519, 0xC935E6, 0xC938B7, 0xC939B9, 0xC93B5D, 0xC93B6A, 0xC96D2C,
    0xC97AE8, 0xC97B5B, 0xC9829B, 0xC982B1, 0xC982B7, 0xC983D6, 0xC987FB, 0xC988BD, 0xC98D93, 0xC98E3F, 0xC992D4,
    0xC99590, 0xC99676, 0xC9984F, 0xC9986D, 0xC99BB4, 0xC99E63, 0xC99F49, 0xC9A122, 0xC9A140, 0xC9A2DD, 0xC9AD11,
    0xC9B6FB, 0xC9BDA9, 0xC9BF7C, 0xC9C137, 0xC9C293, 0xC9C36A, 0xC9C5BC, 0xC9C712, 0xC9CA8F, 0xC9CB6A, 0xC9CDBD,
    0xC9CE4F, 0xC9CF1C, 0xC9CF8B, 0xC9D541, 0xC9DB40, 0xC9DC52, 0xC9E3F1, 0xC9F3A0, 0xC9F3D4, 0xC9F4BE, 0xC9F53F,
    0xC9F594, 0xC9F645, 0xC9F746, 0xC9F9A8, 0xC9FA3E, 0xC9FADC, 0xC9FB31, 0xC9FDD1, 0xC9FE88, 0xC9FECC, 0xCA4188,
    0xCA4A3D, 0xCA6EA9, 0xCA924D, 0xCAB3C4, 0xCAE203, 0xCAE359, 0xCAE574, 0xCAE6B5, 0xCAE892, 0xCAE9F2, 0xCAF27C,
    0xCAF8CE, 0xCAFB71, 0xCB048F, 0xCB168E, 0xCB1D2E, 0xCB1DD3, 0xCB2541, 0xCB25DB, 0xCB2654, 0xCB26B1, 0xCB3E19,
    0xCB4323, 0xCB43C3, 0xCBD66A, 0xCBDA01, 0xCBE02D, 0xCBE292, 0xCC0B60, 0xCC1A26, 0xCC8F99, 0xCCB0E2, 0xCCB11F,
    0xCCBC1C, 0xCCBE46, 0xCCBE80, 0xCD2529, 0xCD2570, 0xCD4659, 0xCD5C41, 0xCD5CCE, 0xCD5D4E, 0xCD5F0D, 0xCD5F8A,
    0xCD62DF, 0xCD6391, 0xCD6EF5, 0xCD872D, 0xCD9EC9, 0xCDB9B8, 0xCDBC1F, 0xCE475D, 0xCE787D, 0xCE7C34, 0xCE7C65,
    0xCEF22B, 0xCF4125, 0xCF6E9E, 0xCFBD47, 0xCFBDC2, 0xCFBDD8, 0xCFBE6A, 0xCFBF61, 0xCFBF94, 0xCFC00C, 0xCFC138,
    0xCFC17C, 0xCFC24B, 0xCFD5F0, 0xCFE63A, 0xCFE9F2, 0xCFFDE4, 0xCFFE65, 0xCFFF19, 0xD0030E, 0xD003D8, 0xD0083A,
    0xD008DD, 0xD09543, 0xD096F4, 0xD098B4, 0xD09B03, 0xD23B18, 0xD23C26, 0xD23C59, 0xD23FE5, 0xD243CA, 0xD2442E,
    0xD25625, 0xD259BF, 0xD259D3, 0xD259E6, 0xD25B7C, 0xD25B9A, 0xD25BB4, 0xD25D32, 0xD261B4, 0xD261EF, 0xD26377,
    0xD2638B, 0xD263A1, 0xD26657, 0xD266CA, 0xD268C3, 0xD27003, 0xD2C4D3, 0xD2C53C, 0xD2C571, 0xD2C810, 0xD32CC0,
    0xD335E9, 0xD3E5B9, 0xD3E8F1, 0xD456EC, 0xD45734, 0xD457B3, 0xD4582E, 0xD45926, 0xD459F2, 0xD45C85, 0xD45D30,
    0xD4713A, 0xD47193, 0xD48D44, 0xD48D82, 0xD492FF, 0xD494E7, 0xD496A6, 0xD49901, 0xD49B10, 0xD49FEF, 0xD4A0E4,
    0xD4A103, 0xD4A264, 0xD4A2B0, 0xD4A42A, 0xD4A82E, 0xD4A8A5, 0xD4AA4C, 0xD4AAD5, 0xD4DF53, 0xD4E03A, 0xD4E3B3,
    0xD4E3C2, 0xD4E3D5, 0xD4E69D, 0xD4E6CC, 0xD4E798, 0xD4E8B1, 0xD4E994, 0xD4E9EC, 0xD4EA2A, 0xD4EDD6, 0xD4F13B,
    0xD4F14F, 0xD4F162, 0xD4F301, 0xD4F31B, 0xD4F335, 0xD4F4C0, 0xD4F977, 0xD4F9AA, 0xD4FB24, 0xD4FB38, 0xD4FB55,
    0xD4FD7D, 0xD4FDE6, 0xD4FFF8, 0xD50728, 0xD511FC, 0xD51285, 0xD51338, 0xD513F8, 0xD5152D, 0xD51CC8, 0xD51D1C,
    0xD51DEC, 0xD51F35, 0xD5205F, 0xD523EE, 0xD5252F, 0xD5281D, 0xD52959, 0xD52CF5, 0xD52D60, 0xD53156, 0xD531D1,
    0xD53762, 0xD53815, 0xD53FB6, 0xD543AC, 0xD54444, 0xD544F9, 0xD54A2F, 0xD54AE2, 0xD54C1E, 0xD54F1D, 0xD5517D,
    0xD5542C, 0xD55B07, 0xD55BEA, 0xD565F9, 0xD56677, 0xD5674B, 0xD567DE, 0xD568E0, 0xD56D00, 0xD56DD3, 0xD56EBB,
    0xD56F7C, 0xD57091, 0xD57CB0, 0xD58CDD, 0xD593D0, 0xD59443, 0xD599D3, 0xD59AB2, 0xD5A504, 0xD5A588, 0xD5A66B,
    0xD5A702, 0xD5A821, 0xD5AC14, 0xD5ACF3, 0xD5ADDE, 0xD5AE9A, 0xD5AFCD, 0xD5BBF0, 0xD5D224, 0xD5D2FD, 0xD5D6F9,
    0xD5DB77, 0xD5EDB1, 0xD5EF39, 0xD60EE5, 0xD6114A, 0xD6118E, 0xD61327, 0xD61562, 0xD67842, 0xD6854E, 0xD70A51,
    0xD70E71, 0xD70E90, 0xD71195, 0xD711B3, 0xD71224, 0xD71339, 0xD7135D, 0xD713C7, 0xD715B7, 0xD71606, 0xD71768,
    0xD71B28, 0xD71D51, 0xD7204F, 0xD72608, 0xD72FFA, 0xD73030, 0xD732C3, 0xD73947, 0xD73B6B, 0xD73BBE, 0xD74177,
    0xD742ED, 0xD743DE, 0xD74635, 0xD746A3, 0xD74B4F, 0xD74C9A, 0xD74D72, 0xD769FF, 0xD76DBD, 0xD76E57, 0xD76FA8,
    0xD77742, 0xD77752, 0xD7775B, 0xD77AA6, 0xD780B3, 0xD78423, 0xD7852E, 0xD7868A, 0xD788D0, 0xD7895A, 0xD98FE6,
    0xD9BC6C, 0xD9BD32, 0xD9BDBE, 0xD9BDE0, 0xD9C6A3, 0xDA31C3, 0xDA698A, 0xDAC3D3, 0xDAC48C, 0xDAC53E, 0xDAC5D4,
    0xDAC693, 0xDAC706, 0xDAC844, 0x10D1844, 0x10D6BE0, 0x10D717E, 0x119A427, 0x120A95B
};