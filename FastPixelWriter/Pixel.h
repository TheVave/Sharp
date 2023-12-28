#pragma once
struct Pixel
{
public:
	// the x pos of the pixel
	short x;
	// the y pos of the pixel
	short y;
	// the color of the pixel with a -127 to 127 range (255 possible values each)
	  char color[3];
};

