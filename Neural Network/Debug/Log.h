#pragma once

class logMatrix {
public:
	static int construct, destruct;
	logMatrix() = delete;
	static void Construct();
	static void Destruct();
	static void Print();
};
