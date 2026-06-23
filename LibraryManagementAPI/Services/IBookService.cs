/* 
    - Changing the IBooKService signature to take into account controller should speak to DTOs only
    - Flow != Controller --> Book Entity --> Service
    - Flow = Controller --> DTOs --> Service --> Entities
    - Service becomes the boundary between API contracts and data models
    - This is because we only want fields explicitly defined in DTOs exposed to the client side
*/

using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Services;

public interface IBookService
{
    // Once in the service layer Skip() and Take() will use the page related params to define pagination
    Task<IEnumerable<BookResponseDTO>> GetAllAsync(
        string? author, // Filtering through the route - e.g. api/books?author=Murakami
        int page, // Which chunk of data is being accessed?
        int pageSize // How many records do we want per page?
    );

    Task<BookResponseDTO> GetByIdAsync(int id);

    // Client should construct DTOs not entities, Service will convert DTO to entity
    Task<BookResponseDTO> CreateAsync(BookCreateDTO dto);

    // Client should provide a DTO not entity, BookMapper will use ApplyUpdate to mutate entity
    Task<BookResponseDTO> UpdateAsync(int id, BookUpdateDTO dto);

    // No DTO needed because destructive process doesn't require data transfer beyond Id
    Task<bool> DeleteAsync(int id);
}
