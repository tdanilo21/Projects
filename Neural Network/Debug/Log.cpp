#include <iostream>
#include "Log.h"

using namespace std;

void logMatrix::Construct() { logMatrix::construct++; }
void logMatrix::Destruct() { logMatrix::destruct++; }
void logMatrix::Print() { cout << logMatrix::construct << ' ' << logMatrix::destruct << endl; }
