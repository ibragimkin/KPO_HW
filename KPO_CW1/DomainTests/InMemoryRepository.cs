//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;
//using Domain.Entities;
//using Domain.Factories;
//using Domain.Interfaces;
//using Domain.Services;

//namespace Tests
//{
//    /// <summary>
//    /// ������� in-memory ���������� ����������� ��� ������.
//    /// ������������, ��� Id ��������� - ��� int (��� ����� ������� �������� � Dictionary<Guid, T>).
//    /// </summary>
//    public class InMemoryRepository<T> : IRepository<T> where T : class
//    {
//        private readonly Dictionary<int, T> _store = new Dictionary<int, T>();
//        private int _counter = 0;

//        public void Add(T entity)
//        {
//            // �������� �������� �������� Id ����� ��������� (���������� ������).
//            // ����� ���������������� Id ������� ��� �������� ��������, ���� ������ ������.
//            var idProperty = entity.GetType().GetProperty("Id");
//            if (idProperty != null && (int)idProperty.GetValue(entity) == 0)
//            {
//                _counter++;
//                idProperty.SetValue(entity, _counter);
//            }

//            var entityId = (int)idProperty.GetValue(entity);
//            _store[entityId] = entity;
//        }

//        public T GetById(int id)
//        {
//            _store.TryGetValue(id, out var entity);
//            return entity;
//        }

//        public IEnumerable<T> GetAll()
//        {
//            return _store.Values;
//        }

//        public void Remove(T entity)
//        {
//            var idProperty = entity.GetType().GetProperty("Id");
//            if (idProperty != null)
//            {
//                int entityId = (int)idProperty.GetValue(entity);
//                _store.Remove(entityId);
//            }
//        }

//        public bool CheckId(int id)
//        {
//            return _store.ContainsKey(id);
//        }
//    }
//}
