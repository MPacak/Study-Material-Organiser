package hr.piprojekt.dao.model;

public enum FileType {
    PDF(1), JPEG(2), UNKNOWN(3);

    private final int id;

    private FileType(int id) {
        this.id = id;
    }


    public int getId() {
        return id;
    }
    public static FileType from(int id) {
        for(FileType value : values()) {
            if(value.id == id ) {
                return value;
            }
        }
        throw new RuntimeException("no such file type");
    }
    public static FileType getFileType(String data) {
        try {
            return FileType.valueOf(data.toUpperCase());
        } catch (IllegalArgumentException e) {
            return FileType.UNKNOWN;
        }
    }
}
