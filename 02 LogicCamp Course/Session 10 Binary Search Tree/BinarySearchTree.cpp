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
void print(Node* root)
{
    queue<pair<Node*, int>> q;
    q.push({root, 0});
    while(!q.empty())
    {
        pair<Node*, int> p = q.front();
        Node* cur = p.first;
        int level = p.second;
        q.pop();
        cout<<cur->val<<" - "<<level<<endl;
        if(cur->left)
        {
            q.push({cur->left, level + 1});
        }
        if(cur->right)
        {
            q.push({cur->right, level + 1});
        }
    }
}
Node* insert(Node* root, int val)
{
    if (root == NULL)
    {
        return new Node(val);
    }
    if (val < root->val)
    {
        root->left = insert(root->left, val);
    }
    else
    {
        root->right = insert(root->right, val);
    }
    return root;
}
void search(Node* root, int key)
{

    if (root == NULL)
    {
        cout<<"Not Found"<<endl;
        return;
    }
    cout<<"Current Node: "<<root->val<<endl;
    if (root->val == key)
    {
        cout<<"Found"<<endl;
        return;
    }
    if (key < root->val)
    {
        search(root->left, key);
    }
    else
    {
        search(root->right, key);
    }
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
    // khujo
    if (root == NULL) return NULL;
    if (root->val == val)
    {
        // case apply hobe
        // case 1
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
            Node* uttoradhikar = findAncestor(root->right);
            root->val = uttoradhikar->val;
            root->right = deleteNode(root->right, uttoradhikar->val);
        }
    }
    // search
    if (val < root->val)
    {
        root->left = deleteNode(root->left, val);
    }
    else if (val > root->val)
    {
        root->right = deleteNode(root->right, val);
    }
    return root;
}
int main()
{
    int n;
    Node* root = NULL;
    while(cin>>n)
    {
        root = insert(root, n);
        if (cin.peek() == '\n')
        {
            break;
        }
    }
    int key; cin>>key;

    // print(root);
    // cout<<"Deleting "<<key<<endl;

    // search(root, key);
    deleteNode(root, key);
    print(root);
    return 0;
}
