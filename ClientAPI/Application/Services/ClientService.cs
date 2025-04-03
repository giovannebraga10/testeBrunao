﻿using ClientAPI.Application.DTOS;
using ClientAPI.Application.Interfaces;
using ClientAPI.Domain.Interfaces;
using ClientAPI.Domain.Models;
using ClientAPI.Models;
using ErrorOr;
using System.Collections.Generic;
using System.Linq;

namespace ClientAPI.Application
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public List<ClienteDto> GetAllClients()
        {
            var clients = _clientRepository.GetAllClients();
            return clients.Select(client => (ClienteDto)client).ToList();
        }

        public ErrorOr<ClienteDto> GetClientById(int id)
        {
            var client = _clientRepository.GetClientById(id);
            if (client is null)
                return Error.NotFound("Cliente não encontrado");

            return (ClienteDto)client;
        }

        public ErrorOr<ClienteDto> AddClient(ClienteDto clientDto)
        {
            var clientResult = Cliente.Create(clientDto.Id, clientDto.Nome, clientDto.Email, clientDto.CPF, clientDto.RG);

            if (clientResult.IsError)
                return clientResult.Errors;

            _clientRepository.AddClient(clientResult.Value);
            return (ClienteDto)clientResult.Value;
        }

        public ErrorOr<ClienteDto> UpdateClient(ClienteDto updatedClientDto)
        {
            var existingClient = _clientRepository.GetClientById(updatedClientDto.Id);
            if (existingClient is null)
                return Error.NotFound("Cliente não encontrado");

            var clientResult = Cliente.Create(updatedClientDto.Id, updatedClientDto.Nome, updatedClientDto.Email, updatedClientDto.CPF, updatedClientDto.RG);

            if (clientResult.IsError)
                return clientResult.Errors;

            _clientRepository.UpdateClient(clientResult.Value);
            return (ClienteDto)clientResult.Value;
        }

        public ErrorOr<bool> DeleteClient(int id)
        {
            var client = _clientRepository.GetClientById(id);
            if (client is null)
                return Error.NotFound("Cliente não encontrado");

            _clientRepository.DeleteClient(id);
            return true;
        }
    }
}
