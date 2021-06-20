#pragma once
#include <vector>

using namespace std;

class MyDouble {
public:
	double val;

	MyDouble(double val = 0);
	MyDouble(const MyDouble& other);

	static MyDouble Random();
	static MyDouble DotProduct(const vector<MyDouble>& a, const vector<MyDouble>& b);

	MyDouble operator=(const MyDouble& other);
	MyDouble operator+=(const MyDouble& other);
	MyDouble operator*(const MyDouble& other) const;
	MyDouble operator+(const MyDouble& other) const;
	MyDouble operator-(const MyDouble& other) const;
};
