export module Main;
#include <iostream>
#include <Windows.h>

using namespace std;
extern "C" {
	int main();
}
int main() {
	MessageBoxW(NULL, L"test", L"also test", MB_OK | MB_ICONINFORMATION);
}