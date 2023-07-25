#pragma once

struct jvs_key_bind {
	int Test;
	int Start;
	int Service;
	int Up;
	int Left;
	int Down;
	int Right;
	int Button1;
	int Button2;
	int Button3;
	int Button4;
	int ArcadeButton1;
	int ArcadeButton2;
	int ArcadeButton3;
	int ArcadeButton4;
	int ArcadeStartButton;
};

void InitializeJvs(jvs_key_bind keyBind);