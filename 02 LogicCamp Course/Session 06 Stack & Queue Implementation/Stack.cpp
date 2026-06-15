#include <bits/stdc++.h>
using namespace std;
class Node
{
public:
    int val;
    Node *next;
    Node *prev;
    Node(int val)
    {
        this->val = val;
        this->next = NULL;
        this->prev = NULL;
    }
};
class LinkedList
{
public:
    Node *head = NULL;
    Node *tail = NULL;
    void insertAtTail(int val)
    {
        Node *newNode = new Node(val);
        if (this->head == NULL)
        {
            this->head = newNode;
            this->tail = newNode;
        }
        else
        {
            this->tail->next = newNode;
            newNode->prev = this->tail;
            this->tail = this->tail->next;
        }
    }
    void deleteAtTail()
    {
        if (this->tail == NULL)
            return;
        if (this->tail->prev == NULL)
        {
            delete this->head;
            this->head = NULL;
            this->tail = NULL;
            return;
        }

        this->tail = this->tail->prev;
        delete this->tail->next;
        this->tail->next = NULL;
    }
};
class MyStack
{
    private:
    LinkedList list;
    int sz = 0;
    public:
    void push(int val)
    {
        list.insertAtTail(val);
        sz++;
    }
    void pop()
    {
        list.deleteAtTail();
        if (sz) sz--;
    }
    int top()
    {
        return list.tail->val;
    }
    int size()
    {
        return sz;
    }
    bool empty()
    {
        return list.head == NULL;
    }
};
int main()
{
    MyStack st;
    st.push(10);
    st.push(20);
    st.push(30);

    // cout<<st.sz<<endl;
    return 0;
}
