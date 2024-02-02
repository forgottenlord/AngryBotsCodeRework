﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    /// <summary>
    /// Передаёт команты клиента на сервер.
    /// </summary>
    [RequireComponent(typeof(NetworkClient))]
    [RequireComponent(typeof(NetworkClientDisplay))]
    public class NetworkInputSync : MonoBehaviour
    {
        [Tooltip("Шаг изменения позиции")]
        [SerializeField] float moveDistance = 1f;
        NetworkClient client;
        NetworkClientDisplay clientMover;

        void Start()
        {
            client = GetComponent<NetworkClient>();
            clientMover = GetComponent<NetworkClientDisplay>();
        }

        void Update()
        {
            if (client.id != "")
            {
                string userInput = GetMoveInput();
                if (userInput != "" && (!clientMover.usersToInterpolate.ContainsKey(gameObject) ||
                    !clientMover.usersToInterpolate[gameObject].isMoving))
                {
                    Move(userInput);
                    client.SendPacket(userInput);
                }
            }
        }

        string GetMoveInput()
        {
            string input = "";
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                input = "a";
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                input = "d";

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                input = "w";
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                input = "s";
            return input;
        }

        public void Move(string userInput)
        {
            // to prevent mismatch between server and client
            if (clientMover.usersToInterpolate.ContainsKey(gameObject) && clientMover.usersToInterpolate[gameObject].isMoving)
                transform.position = client.desiredPosition;
            Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            if (userInput == "a")
                newPos.x -= moveDistance;
            else if (userInput == "d")
                newPos.x += moveDistance;
            else if (userInput == "w")
                newPos.y += moveDistance;
            else if (userInput == "s")
                newPos.y -= moveDistance;

            client.desiredPosition = newPos;
            //interpolate client to newPos
            clientMover.Move(gameObject, newPos);
        }
    }
}