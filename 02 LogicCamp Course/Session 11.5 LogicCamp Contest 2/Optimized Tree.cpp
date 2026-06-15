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

Node* insert(Node* root, int val)
{
    if (root == NULL) return new Node(val);
    if (val < root->val) root->left = insert(root->left, val);
    else root->right = insert(root->right, val);
    return root;
}

Node* findAncestor(Node* parent)
{
    while(parent->left)
    {
        parent = parent->left;
    }
    return parent;
}

Node* deleteNode(Node* root, int val)
{
    if (root == NULL) return NULL;
    if (root->val == val)
    {
        if(root->left == NULL && root->right == NULL)
        {
            delete root;
            return NULL;
        }
        else if(root->left == NULL)
        {
            Node* child = root->right;
            delete root;
            return child;
        }
        else if(root->right == NULL)
        {
            Node* child = root->left;
            delete root;
            return child;
        }
        else
        {
            Node* succession = findAncestor(root->right);
            root->val = succession->val;
            root->right = deleteNode(root->right, succession->val);
        }
    }
    if (val < root->val) root->left = deleteNode(root->left, val);
    else if (val > root->val) root->right = deleteNode(root->right, val);
    return root;
}

// in BST max is always the rightmost node
int findMax(Node* root)
{
    if (root == NULL) return -1;
    while(root->right) root = root->right;
    return root->val;
}

int main()
{
    Node* root = NULL;

    string line;
    getline(cin, line);

    while(line != "")
    {
        istringstream iss(line);
        int val;
        while(iss >> val)
        {
            if(val != -1) root = insert(root, val);
        }
        getline(cin, line);
        if(line.find_first_not_of(" \t\r\n") == string::npos) break;
    }

    int q;
    cin >> q;

    while(q--)
    {
        int type;
        cin >> type;

        if(type == 1)
        {
            int x;
            cin >> x;
            root = insert(root, x);
        }
        else
        {
            int maxVal = findMax(root);
            if(maxVal == -1)
            {
                cout << -1 << endl;
            }
            else
            {
                cout << maxVal << endl;
                root = deleteNode(root, maxVal);
            }
        }
    }

    return 0;
}
