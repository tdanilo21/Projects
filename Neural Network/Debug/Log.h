#pragma once

class logMatrix {
private:
	static int construct, destruct;
public:
	logMatrix() = delete;
	static void Construct();
	static void Destruct();
	static void Print();
};
