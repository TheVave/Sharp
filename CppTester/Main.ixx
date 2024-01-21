export module Main;
#include <iostream>
#include <Windows.h>

using namespace std;
extern "C" {
	__declspec (dllexport) void __cdecl mn() {
		MessageBoxW(NULL, L"test", L"also test", MB_OK | MB_ICONINFORMATION);
	}
}