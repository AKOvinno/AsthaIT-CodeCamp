#include<bits/stdc++.h>
using namespace std;
int countZero(int n)
{
    if(n==0) return 0;
    int baki = countZero(n/10);
    int lastDigit = n%10;
    int amarCnt = 0;
    if(lastDigit==0) amarCnt = 1;
    return baki + amarCnt;
}
int main()
{
    int n;
    cin >> n;
    if(n == 0) {
        cout << 1 << "\n";
        return 0;
    }
    int ans = countZero(n);
    cout << ans << "\n";
    return 0;
}

