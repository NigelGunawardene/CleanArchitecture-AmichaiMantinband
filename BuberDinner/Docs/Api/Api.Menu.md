# Create Menu

### Create Menu Request

```js
POST /hosts/{hostId}/menus
```

```json
{
    "name": "YummyMenu",
    "description": "A menu with yummy food",
    "sections": [
        {
            "name": "Appetizers",
            "description": "Starters",
            "items": [
                {
                    "name": "Pickles",
                    "description": "Deep fried pickles",
                }
            ]
        }
    ]
}
```

### Create Menu Response

```js
200 OK
```

```json
{
    "id": "5f5d7a75-6d35-4cea-80b6-76906143d7f6",
    "name": "YummyMenu",
    "description": "A menu with yummy food",
    "averageRating": null,
    "sections": [
        {
            "name": "Appetizers",
            "description": "Starters",
            "items": [
                {
                    "name": "Pickles",
                    "description": "Deep fried pickles",
                }
            ]
        }
    ]
}
```