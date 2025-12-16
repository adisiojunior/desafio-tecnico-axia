# 🚗 Exemplos para Testar a API de Veículos

Este arquivo contém exemplos de veículos para testar a criação e listagem na API.

## 📋 Como Usar

### 1. Via Swagger UI

1. Acesse `http://localhost:8080/swagger`
2. Expanda o endpoint `POST /api/veiculo`
3. Clique em "Try it out"
4. Cole um dos JSONs abaixo no campo "Request body"
5. Clique em "Execute"

### 2. Via cURL

```bash
curl -X POST "http://localhost:8080/api/veiculo" \
  -H "Content-Type: application/json" \
  -d '{JSON_AQUI}'
```

---

## 🚙 Exemplos de Veículos

### Exemplo 1: Carro Esportivo (Ford Mustang)
```json
{
  "descricao": "Carro esportivo em excelente estado, único dono, revisões em dia",
  "marca": 1,
  "modelo": "Mustang GT",
  "opcionais": "Ar condicionado, Vidros elétricos, Sistema de som premium, Teto solar",
  "valor": 250000.00
}
```

### Exemplo 2: SUV Familiar (Chevrolet Trailblazer)
```json
{
  "descricao": "SUV familiar espaçosa, ideal para viagens",
  "marca": 2,
  "modelo": "Trailblazer",
  "opcionais": "Ar condicionado, GPS, Câmera de ré, Bancos de couro",
  "valor": 180000.00
}
```

### Exemplo 3: Carro Popular (Fiat Uno)
```json
{
  "descricao": "Carro popular econômico, perfeito para cidade",
  "marca": 3,
  "modelo": "Uno",
  "opcionais": null,
  "valor": 45000.00
}
```

### Exemplo 4: Sedan (Volkswagen Jetta)
```json
{
  "descricao": "Sedan confortável e elegante, completo",
  "marca": 4,
  "modelo": "Jetta",
  "opcionais": "Ar condicionado, Bancos de couro, Sensor de estacionamento",
  "valor": 120000.00
}
```

### Exemplo 5: Picape (Toyota Hilux)
```json
{
  "descricao": "SUV robusta e confiável, excelente para estradas",
  "marca": 5,
  "modelo": "Hilux",
  "opcionais": "Ar condicionado, GPS, Câmera de ré, Bancos de couro, Barra de proteção",
  "valor": 220000.00
}
```

### Exemplo 6: Sedan Esportivo (Honda Civic)
```json
{
  "descricao": "Sedan esportivo, motor potente",
  "marca": 6,
  "modelo": "Civic",
  "opcionais": "Ar condicionado, GPS, Bancos de couro, Rodas de liga leve",
  "valor": 150000.00
}
```

### Exemplo 7: SUV Compacta (Nissan Kicks)
```json
{
  "descricao": "SUV compacta, ideal para cidade",
  "marca": 7,
  "modelo": "Kicks",
  "opcionais": "Ar condicionado, GPS, Câmera de ré",
  "valor": 95000.00
}
```

### Exemplo 8: Hatchback (Hyundai HB20)
```json
{
  "descricao": "Hatchback moderno e econômico",
  "marca": 8,
  "modelo": "HB20",
  "opcionais": "Ar condicionado, Direção elétrica",
  "valor": 65000.00
}
```

### Exemplo 9: Sedan Francês (Renault Fluence)
```json
{
  "descricao": "Sedan francês, confortável e elegante",
  "marca": 9,
  "modelo": "Fluence",
  "opcionais": "Ar condicionado, GPS, Bancos de couro",
  "valor": 85000.00
}
```

### Exemplo 10: SUV Francesa (Peugeot 3008)
```json
{
  "descricao": "SUV francesa, espaçosa e confortável",
  "marca": 10,
  "modelo": "3008",
  "opcionais": "Ar condicionado, GPS, Câmera de ré, Bancos de couro, Teto solar",
  "valor": 140000.00
}
```

---

## 🔍 Testando a Listagem com Filtros

### Listar todos (primeira página)
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10
```

### Filtrar por Marca (Ford = 1)
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&marca=1
```

### Filtrar por Modelo (busca parcial)
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&modelo=Mustang
```

### Filtrar por Faixa de Valor
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&valorMin=100000&valorMax=200000
```

### Filtrar por Múltiplos Critérios
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&marca=1&valorMin=200000&valorMax=300000&orderBy=valor&sortOrder=desc
```

### Ordenar por Valor (maior para menor)
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&orderBy=valor&sortOrder=desc
```

### Ordenar por Data de Criação (mais recentes primeiro)
```
GET http://localhost:8080/api/veiculo?page=1&pageSize=10&orderBy=CreatedAt&sortOrder=desc
```

---

## 📊 Resposta Esperada da Listagem

```json
{
  "data": [
    {
      "id": "b93d9805-4857-40f9-b297-fad04f3a3b42",
      "descricao": "Carro esportivo em excelente estado",
      "marca": 1,
      "modelo": "Mustang GT",
      "opcionais": "Ar condicionado, Vidros elétricos",
      "valor": 250000
    }
  ],
  "page": 1,
  "pageSize": 10,
  "totalCount": 15,
  "totalPages": 2,
  "hasPrevious": false,
  "hasNext": true
}
```

---

## 🎯 Dicas de Teste

1. **Crie vários veículos** usando os exemplos acima
2. **Teste a paginação**: crie mais de 10 veículos e teste `page=1`, `page=2`, etc.
3. **Teste os filtros**: combine marca, modelo e valor
4. **Teste a ordenação**: ordene por valor, modelo, data
5. **Teste soft delete**: delete um veículo e verifique que ele não aparece mais na listagem

---

## 📝 Valores de Marca

- 1 = Ford
- 2 = Chevrolet
- 3 = Fiat
- 4 = Volkswagen
- 5 = Toyota
- 6 = Honda
- 7 = Nissan
- 8 = Hyundai
- 9 = Renault
- 10 = Peugeot

