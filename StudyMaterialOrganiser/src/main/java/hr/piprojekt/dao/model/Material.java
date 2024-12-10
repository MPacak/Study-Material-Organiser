package hr.piprojekt.dao.model;

public class Material {
    private int id;
    private String name;
    private String description;
    private String Link;
    private String filepath;
    private FileType filetype;

    public Material() {
    }

    public Material(String name, String description, String Link, String filepath, FileType filetype) {
        this.name = name;
        this.description = description;
        this.Link = Link;
        this.filepath = filepath;
        this.filetype = filetype;
    }

    public Material(int id, String name, String description, String Link,
                    String filepath, FileType filetype) {
        this(name, description,Link,filepath, filetype);
        this.id = id;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getDescription() {
        return description;
    }

    public String getLink() {
        return Link;
    }

    public String getFilepath() {
        return filepath;
    }

    public FileType getFiletype() {
        return filetype;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public void setLink(String Link) {
        this.Link = Link;
    }

    public void setFilepath(String filepath) {
        this.filepath = filepath;
    }

    public void setFiletype(FileType filetype) {
        this.filetype = filetype;
    }
}
