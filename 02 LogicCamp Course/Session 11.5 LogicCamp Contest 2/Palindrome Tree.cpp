#include<bits/stdc++.h>
using namespace std;

class Node
{
    public:
        int val;
        Node* left;
        Node* right;
    Node(int val)
    {
        this->val = val;
        this->left = NULL;
        this->right = NULL;
    }
};

string S;
int ans = 0;

bool isPalindrome(string str)
{
    int l = 0, r = str.size() - 1;
    while(l < r)
    {
        if(str[l] != str[r]) return false;
        l++; r--;
    }
    return true;
}

void dfs(Node* root, string path)
{
    if(root == NULL) return;

    path += S[root->val];

    if(root->left == NULL && root->right == NULL)
    {
        if(isPalindrome(path)) ans++;
        return;
    }

    dfs(root->left, path);
    dfs(root->right, path);
}

void input(Node* &root)
{
    int val;
    cin >> val;

    if(val == -1) return;
    root = new Node(val);
    queue<Node*> q;
    q.push(root);
    while(!q.empty())
    {
        Node* cur = q.front();
        q.pop();
        int left, right;
        cin >> left >> right;
        if(left != -1)
        {
            cur->left = new Node(left);
            q.push(cur->left);
        }
        if(right != -1)
        {
            cur->right = new Node(right);
            q.push(cur->right);
        }
    }
}

int main()
{
    cin >> S;
    Node* root = NULL;
    input(root);
    dfs(root, "");
    cout << ans << endl;
    return 0;
}
