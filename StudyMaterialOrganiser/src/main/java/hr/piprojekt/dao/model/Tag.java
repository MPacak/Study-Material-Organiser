package hr.piprojekt.dao.model;

import java.util.List;

public class Tag {

    private int id;


    private String tagName;

    private List<MaterialTag> materialTags;

    // Getters and Setters
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTagName() {
        return tagName;
    }

    public void setTagName(String tagName) {
        this.tagName = tagName;
    }

    public List<MaterialTag> getMaterialTags() {
        return materialTags;
    }

    public void setMaterialTags(List<MaterialTag> materialTags) {
        this.materialTags = materialTags;
    }
}
