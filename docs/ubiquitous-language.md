# Ubiquitous Language (Glossary)

- Food: A menu item offered for sale; aggregate root in Menu BC.
- FoodType: Categorization of Food; aggregate root in Menu BC.
- Money: Value object representing price (Amount, Currency) with rounding rules.
- FoodStatus: VO indicating availability state of Food (Active, Discontinued).
- Stock: A storage location (e.g., warehouse, fridge); aggregate root in Inventory BC.
- Ingredients: Raw material used in recipes; aggregate root in Inventory BC.
- FoodRecipe: Relationship between Food and Ingredients with required Measurement.
- StockItems: Quantity of a specific Ingredients at a Stock; aggregate root with capacity and status (Available, LowStock, Restocking, OutOfStock).
- Measurement: VO of quantity and unit.
- Capacity: VO of maximum allowed measurement for a StockItems.

Rules (informal)
- Money amount follows currency-specific precision (USD 2dp, VND 0dp).
- StockItems cannot exceed Capacity; status is derived from quantity/capacity ratio.
- Food emits events on creation, status change, updates, and delete.

DTO Conventions
- Requests use primitive/DTO fields; mapping to Domain VOs happens in Application.
- Responses reflect domain terms but remain DTOs.
