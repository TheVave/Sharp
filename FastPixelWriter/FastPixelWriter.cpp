// FastPixelWriter.cpp : Defines the entry point for the application.
//

#include "framework.h"
#include "FastPixelWriter.h"
#include "Pixel.h"
#include <cstddef>
#include <string>
#include <atlstr.h>
#include <vector>
using namespace std;

#define MAX_LOADSTRING 100
#define MY_BUFSIZE 1024

// Global Variables:
HINSTANCE hInst;                                // current instance
WCHAR szTitle[MAX_LOADSTRING];                  // The title bar text 
WCHAR szWindowClass[MAX_LOADSTRING];            // the main window class name

// WM_PAINT message
RECT actual_wnd_rect;
int width;
int height;
RECT wnd_rect;
int x = 0;
int y = 0;
int intx = 0;
int inty = 0;
int area = 0;
bool standardDisplay = true;
int ImageSize;
UCHAR* ImageData{};
BITMAPINFO bmpi;
BITMAPINFOHEADER bmih{};

//wWinMain created for WM_PAINT message
Pixel pixels[];
int pixels_length;

// wWinMain vars
string ptr1{};
string ptr2{};
Pixel* pixelarr1{};
string dims{};

// Forward declarations of functions included in this code module:
ATOM                MyRegisterClass(HINSTANCE hInstance);
BOOL                InitInstance(HINSTANCE, int);
LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);
INT_PTR CALLBACK    About(HWND, UINT, WPARAM, LPARAM);

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPWSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	string str = { CW2A(lpCmdLine) };
	// override for debugging, disables looking for pixels and just shows nothing.
	if (str != "false") {
		// I'm doing a lot of error stuff because I want to not have this window close with no warning.
		try {
			// setting up pointers and dim strings
			ptr1 = str.substr(str.find(',') + 1);
			dims = str.substr(0, str.find(','));
			// the ptr is passed as a hex string, so this is converting it to a int, then a Pixel*
			pixelarr1 = (Pixel*)HNSTI(ptr1);
		}
		catch (exception e) {
			MessageBoxA(NULL, "Error: Incorrect params, there was an error loading the pixels to display.", "Error!", MB_ICONERROR | MB_OK);
		};
		try {
			// managing wanted window dims
			// initilizing vars
			int width{};
			int height{};
			int arrayLength{};
			// converting the string to a num, simlar to the Int.Parse method in C#
			width = stoi(dims.substr(0, dims.find("x")));
			height = stoi(dims.substr(dims.find("x") + 1));
			// finding the length of the array (x3 because there is a UCHAR for each color in the image
			arrayLength = width * height * 3;
			// setting the target rect for the window
			wnd_rect.left = 0;
			wnd_rect.top = 0;
			wnd_rect.right = width;
			wnd_rect.bottom = height;
		}
		catch (exception e) {
			MessageBoxA(NULL, "Error setting window size", "Error!", MB_ICONERROR | MB_OK);
		}
		// beginning the conversion of the Pixel* to a Pixel[]
		/*try {
			try {
				MessageBoxA(NULL, ("debug info: " + to_string(pixelarr1->x) + " " + to_string(pixelarr1->y) + " " + 
					to_string((int)pixelarr1->color[0]) + " " + to_string((int)pixelarr1->color[1]) + " " +
					to_string((int)pixelarr1->color[2])).c_str(), "Error!", MB_OK);
			}
			catch (exception e) {
				MessageBoxA(NULL, ("Error: Incorrect type exact error: " + ((string)e.what())).c_str(), "Error!", MB_ICONERROR | MB_OK);
			}
		}
		catch (exception e) {
			MessageBoxA(NULL, ("Error: Unknown error, conversion from a Pixel* to Pixel[] failed, exact error: " + ((string)e.what())).c_str(), "Error!", MB_ICONERROR | MB_OK);
		}*/
	}
	else standardDisplay = false;
	// BITMAPINFOHEADER forward declerations for speed reasons
	bmih.biBitCount = 24;
	bmih.biClrImportant = 0;
	bmih.biClrUsed = 0;
	bmih.biCompression = BI_RGB;
	bmih.biWidth = width;
	bmih.biHeight = height;
	bmih.biPlanes = 1;
	bmih.biSize = sizeof(BITMAPINFOHEADER);
	bmih.biSizeImage = ImageSize;

	// setting the BITMAPINFO object to hold the BITMAPINFOHEADER above
	bmpi.bmiHeader = bmih;

	// Initialize global strings
	LoadStringW(hInstance, IDS_APP_TITLE, szTitle, MAX_LOADSTRING);
	LoadStringW(hInstance, IDC_FASTPIXELWRITER, szWindowClass, MAX_LOADSTRING);
	MyRegisterClass(hInstance);

	// Perform application initialization:
	if (!InitInstance(hInstance, nCmdShow))
	{
		return FALSE;
	}

	HACCEL hAccelTable = LoadAccelerators(hInstance, MAKEINTRESOURCE(IDC_FASTPIXELWRITER));

	MSG msg;

	// Main message loop:
	while (GetMessage(&msg, nullptr, 0, 0))
	{
		if (!TranslateAccelerator(msg.hwnd, hAccelTable, &msg))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return (int)msg.wParam;
}



//
//  FUNCTION: MyRegisterClass()
//
//  PURPOSE: Registers the window class.
//
ATOM MyRegisterClass(HINSTANCE hInstance)
{
	WNDCLASSEXW wcex;

	wcex.cbSize = sizeof(WNDCLASSEX);

	wcex.style = CS_HREDRAW | CS_VREDRAW;
	wcex.lpfnWndProc = WndProc;
	wcex.cbClsExtra = 0;
	wcex.cbWndExtra = 0;
	wcex.hInstance = hInstance;
	wcex.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_FASTPIXELWRITER));
	wcex.hCursor = LoadCursor(nullptr, IDC_ARROW);
	wcex.hbrBackground = (HBRUSH)(COLOR_WINDOW + 1);
	wcex.lpszMenuName = MAKEINTRESOURCEW(IDC_FASTPIXELWRITER);
	wcex.lpszClassName = szWindowClass;
	wcex.hIconSm = LoadIcon(wcex.hInstance, MAKEINTRESOURCE(IDI_SMALL));

	return RegisterClassExW(&wcex);
}

//
//   FUNCTION: InitInstance(HINSTANCE, int)
//
//   PURPOSE: Saves instance handle and creates main window
//
//   COMMENTS:
//
//        In this function, we save the instance handle in a global variable and
//        create and display the main program window.
//
BOOL InitInstance(HINSTANCE hInstance, int nCmdShow)
{
	hInst = hInstance; // Store instance handle in our global variable

	HWND hWnd = CreateWindowW(szWindowClass, szTitle, WS_OVERLAPPEDWINDOW,
		CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);

	if (!hWnd)
	{
		return FALSE;
	}

	ShowWindow(hWnd, nCmdShow);
	UpdateWindow(hWnd);

	return TRUE;
}

//
//  FUNCTION: WndProc(HWND, UINT, WPARAM, LPARAM)
//
//  PURPOSE: Processes messages for the main window.
//
//  WM_COMMAND  - process the application menu
//  WM_PAINT    - Paint the main window
//  WM_DESTROY  - post a quit message and return
//
//
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch (message)
	{
	case WM_COMMAND:
	{
		int wmId = LOWORD(wParam);
		// Parse the menu selections:
		switch (wmId)
		{
		case IDM_ABOUT:
			DialogBox(hInst, MAKEINTRESOURCE(IDD_ABOUTBOX), hWnd, About);
			break;
		case IDM_EXIT:
			DestroyWindow(hWnd);
			break;
		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
		}
	}
	break;
	case WM_PAINT:
	{
		PAINTSTRUCT ps;
		HDC hdc = BeginPaint(hWnd, &ps);
		if (standardDisplay) AdjustWindowRect(&wnd_rect, WS_MAXIMIZE,false);
		
		HWND hwnd = WindowFromDC(hdc);
		GetWindowRect(hwnd, &actual_wnd_rect);

		// finding actual width & height
		width = actual_wnd_rect.right - actual_wnd_rect.left;
		height = actual_wnd_rect.bottom - actual_wnd_rect.top;

		// finding the image size (in memory in bytes / UCHARs)
		// UCHAR has a size of 1 byte in memory with a range of 0-255 (null char to weird y char)
		ImageSize = width * height * 3;

		// setting the image info for the SetDIBitsToDevice method to interpret
		bmih.biWidth = width;

		bmih.biHeight = height;

		bmih.biSizeImage = ImageSize;

		// reassigning the BITMAPINFO var so that the data above isn't just floating around in memory.
		bmpi.bmiHeader = bmih;

		// creating a new ImageData array with the 
		ImageData = new UCHAR[ImageSize];

		for (int i = 0; i < ImageSize - 3; i += 3)
		{
			// finding x and y with some math
			y = ceil(i / 3 / width);
			x = (i / 3) - (y * width);
			// if it doesn't have any pixels to display it just displays a grey color.
			// 
			// though it would be testing something that stays the same for every single frame,
			// I'm leaving there because I am not needing to do anything about it right now.
			if (!standardDisplay) {
				// setting the color of the pixels to the dark windows theme (about) (32,32,32)
				ImageData[i] = 32;
				ImageData[i + 1] = 32;
				ImageData[i + 2] = 32;
			}
			else {
				// gets pixel data from SharpPhysics (MainRenderer.cs -> _2dRenderer.cs or to _3dRenderer.cs (not currently implemented)


			}
		}

		
		SetDIBitsToDevice(hdc, 0, 0, width, height, 0, 0, 0, height, ImageData, &bmpi, DIB_RGB_COLORS);

		EndPaint(hWnd, &ps);
	}
	break;
	case WM_DESTROY:
		PostQuitMessage(0);
		break;
	default:
		return DefWindowProc(hWnd, message, wParam, lParam);
	}
	return 0;
}

// Message handler for about box.
INT_PTR CALLBACK About(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam)
{
	UNREFERENCED_PARAMETER(lParam);
	switch (message)
	{
	case WM_INITDIALOG:
		return (INT_PTR)TRUE;

	case WM_COMMAND:
		if (LOWORD(wParam) == IDOK || LOWORD(wParam) == IDCANCEL)
		{
			EndDialog(hDlg, LOWORD(wParam));
			return (INT_PTR)TRUE;
		}
		break;
	}
	return (INT_PTR)FALSE;
}
int HNSTI(string hxstr) {
	return std::stoul(hxstr, nullptr, 16);
}